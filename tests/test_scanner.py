"""Tests for mobile-mashup-kit."""
import zipfile
import tempfile
from pathlib import Path

from mobile_mashup_kit.scanner import scan_apk


def test_scan_apk_unity_il2cpp(tmp_path):
    """Test detecting Unity IL2CPP from an APK-like structure."""
    # Create a fake APK with IL2CPP indicators
    apk_path = tmp_path / "game.apk"
    with zipfile.ZipFile(apk_path, "w") as z:
        z.writestr("lib/arm64-v8a/libil2cpp.so", "fake")
        z.writestr("assets/bin/Data/Managed/global-metadata.dat", "fake")

    result = scan_apk(apk_path)
    assert result.engine == "unity-il2cpp"
    assert "arm64-v8a" in result.architectures


def test_scan_apk_unity_mono(tmp_path):
    """Test detecting Unity Mono from an APK-like structure."""
    apk_path = tmp_path / "game.apk"
    with zipfile.ZipFile(apk_path, "w") as z:
        z.writestr("lib/arm64-v8a/libmono.so", "fake")
        z.writestr("assets/bin/Data/Managed/Assembly-CSharp.dll", "fake")

    result = scan_apk(apk_path)
    assert result.engine == "unity-mono"
    assert "arm64-v8a" in result.architectures


def test_scan_apk_cocos_lua(tmp_path):
    """Test detecting Cocos2d-x Lua from an APK-like structure."""
    apk_path = tmp_path / "game.apk"
    with zipfile.ZipFile(apk_path, "w") as z:
        z.writestr("lib/arm64-v8a/libcocos2d.so", "fake")
        z.writestr("assets/main.lua", "fake")

    result = scan_apk(apk_path)
    assert result.engine == "cocos-lua"


def test_scan_apk_unknown(tmp_path):
    """Test detecting unknown engine."""
    apk_path = tmp_path / "game.apk"
    with zipfile.ZipFile(apk_path, "w") as z:
        z.writestr("lib/arm64-v8a/libgame.so", "fake")

    result = scan_apk(apk_path)
    assert result.engine == "unknown"


def test_scan_directory_unity_il2cpp(tmp_path):
    """Test scanning a decompiled directory."""
    # Create a fake decompiled directory
    decomp = tmp_path / "decompiled"
    (decomp / "lib" / "arm64-v8a").mkdir(parents=True)
    (decomp / "lib" / "arm64-v8a" / "libil2cpp.so").touch()
    (decomp / "assets" / "bin" / "Data" / "Managed").mkdir(parents=True)
    (decomp / "assets" / "bin" / "Data" / "Managed" / "global-metadata.dat").touch()

    result = scan_apk(decomp)
    assert result.engine == "unity-il2cpp"
    assert "arm64-v8a" in result.architectures
