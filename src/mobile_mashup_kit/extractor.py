"""Asset extractor for pulling textures, audio, and meshes from APKs."""
import zipfile
from pathlib import Path
from typing import Optional


class AssetExtractor:
    """Extracts game assets from an APK."""

    def __init__(self, output_dir: Path):
        self.output_dir = Path(output_dir)
        self.output_dir.mkdir(parents=True, exist_ok=True)

    def extract(self, apk_path: Path) -> Path:
        """Extract assets from an APK."""
        out = self.output_dir / apk_path.stem
        out.mkdir(exist_ok=True)

        if apk_path.is_dir():
            return self._extract_from_dir(apk_path, out)
        else:
            return self._extract_from_apk(apk_path, out)

    def _extract_from_apk(self, apk_path: Path, out: Path) -> Path:
        """Extract assets from an APK file."""
        with zipfile.ZipFile(apk_path, "r") as z:
            asset_extensions = {
                ".png", ".jpg", ".jpeg", ".gif", ".bmp", ".tga", ".webp",
                ".ogg", ".mp3", ".wav", ".flac", ".m4a",
                ".obj", ".fbx", ".gltf", ".glb",
                ".ttf", ".otf",
                ".json", ".xml", ".csv", ".txt",
            }
            for name in z.namelist():
                if any(name.lower().endswith(ext) for ext in asset_extensions):
                    target = out / name
                    target.parent.mkdir(parents=True, exist_ok=True)
                    with z.open(name) as src, open(target, "wb") as dst:
                        dst.write(src.read())
        return out

    def _extract_from_dir(self, dir_path: Path, out: Path) -> Path:
        """Extract assets from a decompiled directory."""
        import shutil
        asset_extensions = {
            ".png", ".jpg", ".jpeg", ".gif", ".bmp", ".tga", ".webp",
            ".ogg", ".mp3", ".wav", ".flac", ".m4a",
            ".obj", ".fbx", ".gltf", ".glb",
            ".ttf", ".otf",
            ".json", ".xml", ".csv", ".txt",
        }
        for f in dir_path.rglob("*"):
            if f.is_file() and f.suffix.lower() in asset_extensions:
                rel = f.relative_to(dir_path)
                target = out / rel
                target.parent.mkdir(parents=True, exist_ok=True)
                shutil.copy2(f, target)
        return out
