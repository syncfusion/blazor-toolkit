import importlib.util
import pathlib
import unittest

spec = importlib.util.spec_from_file_location("gate", pathlib.Path(__file__).with_name("gate.py"))
gate = importlib.util.module_from_spec(spec)
spec.loader.exec_module(gate)


def contract():
    return {"identity": {}, "surface": ["T Public", "M Public.Required()"],
            "callbacks": {}, "parameters": {}, "wire": {}, "generated": {}}


class GateTests(unittest.TestCase):
    def test_rejects_new_missing_public_member(self):
        expected = contract()
        actual = dict(expected, surface=["T Public"])
        with self.assertRaisesRegex(ValueError, "surface"):
            gate.compare_contract(expected, actual)

    def test_rejects_changed_callback_and_wire_value(self):
        for key in ["identity", "callbacks", "parameters", "wire", "generated"]:
            with self.subTest(key=key), self.assertRaisesRegex(ValueError, key):
                gate.compare_contract(dict(contract(), **{key: {"x": "old"}}),
                                      dict(contract(), **{key: {"x": "new"}}))

    def test_rejects_empty_or_missing_contract(self):
        for value in [{}, {"surface": ["T Required"]}, dict(contract(), surface=[])]:
            with self.subTest(value=value), self.assertRaises(ValueError):
                gate.compare_contract(contract(), value)

    def test_rejects_empty_subset_duplicate_or_unknown_frameworks(self):
        for value in [[], ["net8.0"], ["net8.0"] * 3, ["net8.0", "net9.0", "net11.0"]]:
            with self.subTest(value=value), self.assertRaises(ValueError):
                gate.validate_baseline({"frameworks": value})

    def test_accepts_identical_contract(self):
        value = contract()
        gate.compare_contract(value, value)

    def test_frozen_probe_includes_protected_nested_type(self):
        import gzip
        import json
        for tfm in ["net8.0", "net9.0", "net10.0"]:
            with gzip.open(pathlib.Path(__file__).with_name(f"reviewed-{tfm}.json.gz"), "rt") as stream:
                value = json.load(stream)
            self.assertTrue(any(row.startswith("T ") and "+ButtonGroups" in row for row in value["surface"]))
            changed = dict(value, surface=[row for row in value["surface"] if "+ButtonGroups" not in row])
            with self.assertRaisesRegex(ValueError, "surface"):
                gate.compare_contract(value, changed)

    def test_rejects_nested_boolean_to_number_changes(self):
        for old, new in [(True, 1), (False, 0)]:
            with self.subTest(old=old), self.assertRaisesRegex(ValueError, "wire"):
                gate.compare_contract(dict(contract(), wire={"nested": [{"value": old}]}),
                                      dict(contract(), wire={"nested": [{"value": new}]}))

    def test_rejects_wrong_baseline_bytes(self):
        with self.assertRaisesRegex(ValueError, "SHA-256"):
            gate.verify_digest(b"wrong release", "0" * 64)

    def test_verifies_digest(self):
        gate.verify_digest(b"", "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855")


if __name__ == "__main__":
    unittest.main()
