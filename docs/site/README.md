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

The site mirrors Gallery's displayed snippets. It does not automatically verify that every snippet reproduces its preview or that all claims on the page are production-ready. The package is currently a local preview, not an instruction to publish it.
