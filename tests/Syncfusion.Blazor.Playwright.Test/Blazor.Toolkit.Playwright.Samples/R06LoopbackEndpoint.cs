using System.Net;
using System.Text;

namespace Blazor.Toolkit.Playwright.Samples;

// Test host only. Not registered unless both Development and the explicit opt-in are set.
internal static class R06LoopbackEndpoint
{
    internal static void Map(WebApplication app)
    {
        if (!app.Environment.IsDevelopment() || app.Configuration["R06_CHARACTERIZATION"] != "1") return;
        var sync = new object();
        var chunks = new List<int>();
        var release = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        int holdChunk = app.Configuration["R06_HOLD_CHUNK"] switch { "1" => 1, "2" => 2, _ => 0 };
        bool hasHeld = false;
        int active = 0;
        int admitted = 0;
        int completed = 0;
        bool timedOut = false;
        bool Local(HttpContext c)
        {
            // Preserve fixture error statuses instead of re-executing the host's Razor not-found page.
            var pages = c.Features.Get<Microsoft.AspNetCore.Diagnostics.IStatusCodePagesFeature>();
            if (pages != null) pages.Enabled = false;
            return c.Connection.RemoteIpAddress is { } ip && IPAddress.IsLoopback(ip)
                && c.Request.Host.Host is "localhost" or "127.0.0.1" or "::1";
        }

        app.MapGet("/r06-live/state", (HttpContext c) =>
        {
            if (!Local(c)) return Results.NotFound();
            lock (sync) return Results.Json(new { chunks = chunks.ToArray(), active, completed, timedOut });
        });
        app.MapPost("/r06-live/release", (HttpContext c) =>
        {
            if (!Local(c) || c.Request.Headers["X-R06-Control"] != "DUMMY_CONTROL") return Results.NotFound();
            release.TrySetResult(true);
            return Results.Ok();
        });
        app.MapPost("/r06-live/save", async (HttpContext c) =>
        {
            if (!Local(c)) return Results.NotFound();
            // Prevent unsolicited cross-origin browser requests to the opt-in endpoint.
            if (c.Request.Headers.Origin != "http://localhost:5000") return Results.StatusCode(403);
            if (c.Request.ContentLength is not long length || length <= 0 || length > 8192) return Results.StatusCode(413);
            lock (sync)
            {
                if (admitted >= 16) return Results.StatusCode(429);
                admitted++;
                active++;
            }
            using var timeout = CancellationTokenSource.CreateLinkedTokenSource(c.RequestAborted);
            timeout.CancelAfter(TimeSpan.FromSeconds(15));
            try
            {
                // Fixed allocation and bounded read; never files, form buffering or persistence.
                var buffer = new byte[8193];
                int used = 0;
                while (used < buffer.Length)
                {
                    int read = await c.Request.Body.ReadAsync(buffer.AsMemory(used), timeout.Token);
                    if (read == 0) break;
                    used += read;
                }
                if (used > 8192) return Results.StatusCode(413);
                var body = Encoding.ASCII.GetString(buffer, 0, used);
                var marker = "name=\"chunk-index\"\r\n\r\n";
                int index = body.IndexOf(marker, StringComparison.Ordinal);
                if (!body.Contains("filename=\"A.txt\"", StringComparison.Ordinal) || index < 0 || index + marker.Length >= body.Length) return Results.BadRequest();
                int chunk = body[index + marker.Length] - '0';
                if (chunk is < 0 or > 2) return Results.BadRequest();
                int fileStart = body.IndexOf("\r\n\r\n", body.IndexOf("filename=", StringComparison.Ordinal), StringComparison.Ordinal) + 4;
                int fileEnd = body.IndexOf("\r\n--", fileStart, StringComparison.Ordinal);
                if (fileEnd - fileStart != 1024 || body.AsSpan(fileStart, 1024).IndexOfAnyExcept((char)('A' + chunk)) >= 0) return Results.BadRequest();
                bool hold;
                lock (sync) { hold = !hasHeld && chunk == holdChunk; if (hold) hasHeld = true; chunks.Add(chunk); }
                if (hold) await release.Task.WaitAsync(timeout.Token);
                lock (sync) completed++;
                return Results.Text("DUMMY_RESPONSE");
            }
            catch (OperationCanceledException)
            {
                lock (sync) timedOut |= !c.RequestAborted.IsCancellationRequested;
                return Results.StatusCode(408);
            }
            finally { lock (sync) active--; }
        });
    }
}
