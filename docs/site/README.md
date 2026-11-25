# ChartNivra documentation site

This is a static HTML, CSS, and JavaScript site. Open `index.html` in a browser or serve this folder with a local static HTTP server:

```sh
python -m http.server 8000 --directory docs/site
```

Then open `http://localhost:8000`.

## Gallery data

The site's card metadata and displayed snippets are generated from the development Gallery. From the repository root, run:

```sh
python tools/sync_gallery_site.py
```

Commit the resulting `assets/js/gallery-data.js` alongside Gallery changes. The generator has a card-key mapping and stops when the card count or expected source snippets change; update the mapping when adding or reordering cards. The two studio pages are C#-only and expose their page source instead of a XAML snippet.

The site mirrors Gallery's displayed snippets. It does not automatically verify that every snippet reproduces its preview or that all claims on the page are production-ready. [ChartNivra 1.0.0](https://www.nuget.org/packages/ChartNivra/1.0.0) is available on NuGet.org; the PackageSmoke project may still use a local preview for testing.

## GitHub Pages deployment

The `.github/workflows/deploy-docs.yml` workflow publishes this folder when a tag beginning with `v` is pushed and the tagged commit is already reachable from `main`. Push the commit to `main` before pushing the tag, for example:

```sh
git switch main
git pull --ff-only origin main
git tag v1.0.0
git push origin v1.0.0
```

In the repository's **Settings → Pages**, set **Build and deployment → Source** to **GitHub Actions**. If the `github-pages` environment has deployment restrictions, ensure they allow the release tag. The workflow publishes the files stored at the tag; it does not build or publish a NuGet package.
