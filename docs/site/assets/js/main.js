/* ═══════════════════════════════════════════════════════════
   ChartNivra Documentation — Interactive JS
   Clean, minimal, professional
═══════════════════════════════════════════════════════════ */

'use strict';

/* ── NAV scroll shadow ──────────────────────────────────── */
window.addEventListener('scroll', () => {
  document.getElementById('nav').classList.toggle('scrolled', window.scrollY > 10);
}, { passive: true });

/* ── Copy install command ───────────────────────────────── */
function copyInstall(btn) {
  const cmd = document.querySelector('.install-cmd').textContent;
  navigator.clipboard.writeText(cmd).then(() => {
    btn.innerHTML = `<svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="#22C55E" stroke-width="2.5"><polyline points="20 6 9 17 4 12"/></svg>`;
    setTimeout(() => {
      btn.innerHTML = `<svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><rect x="9" y="9" width="13" height="13" rx="2" ry="2"/><path d="M5 15H4a2 2 0 01-2-2V4a2 2 0 012-2h9a2 2 0 012 2v1"/></svg>`;
    }, 2000);
  });
}

/* ── Copy code snippet ──────────────────────────────────── */
function copySnippet(btn) {
  const pre = btn.closest('.code-block').querySelector('pre code');
  navigator.clipboard.writeText(pre.textContent).then(() => {
    const orig = btn.textContent;
    btn.textContent = 'Copied!';
    btn.style.color = '#22C55E';
    setTimeout(() => { btn.textContent = orig; btn.style.color = ''; }, 2000);
  });
}

/* ── Chart category tabs ────────────────────────────────── */
document.querySelectorAll('.tab-btn').forEach(btn => {
  btn.addEventListener('click', () => {
    document.querySelectorAll('.tab-btn').forEach(b => b.classList.remove('active'));
    btn.classList.add('active');

    const cat = btn.dataset.category;
    document.querySelectorAll('.chart-card').forEach(card => {
      if (cat === 'all' || card.dataset.category === cat) {
        card.classList.remove('hidden');
      } else {
        card.classList.add('hidden');
      }
    });
  });
});

/* ── Palette switcher for phone showcase ────────────────── */
const paletteColors = {
  blue:    { primary: '#0EA5E9', light: '#38BDF8', lighter: '#7DD3FC' },
  violet:  { primary: '#8B5CF6', light: '#A78BFA', lighter: '#C4B5FD' },
  emerald: { primary: '#059669', light: '#34D399', lighter: '#6EE7B7' },
  rose:    { primary: '#E11D48', light: '#FB7185', lighter: '#FDA4AF' },
  amber:   { primary: '#D97706', light: '#FBBF24', lighter: '#FDE68A' },
};

function switchPalette(palette, btn) {
  document.querySelectorAll('.palette-btn').forEach(b => b.classList.remove('active'));
  btn.classList.add('active');

  const c = paletteColors[palette];
  if (!c) return;

  // Update all chart SVG strokes in the phone mockups
  document.querySelectorAll('.phone-mockup .pm-screen svg [stroke]').forEach(el => {
    const s = el.getAttribute('stroke');
    if (s && s.startsWith('#') && s !== '#F1F5F9' && s !== '#E2E8F0' && s !== '#CBD5E1' && s !== '#94A3B8') {
      if (['#0EA5E9','#38BDF8','#7DD3FC','#8B5CF6','#A78BFA','#C4B5FD','#059669','#34D399','#E11D48','#FB7185','#D97706','#FBBF24'].includes(s)) {
        el.setAttribute('stroke', c.primary);
      }
    }
  });

  // Update fills
  document.querySelectorAll('.phone-mockup .pm-screen svg [fill]').forEach(el => {
    const f = el.getAttribute('fill');
    if (f && f.startsWith('#')) {
      if (['#0EA5E9','#38BDF8'].includes(f)) el.setAttribute('fill', c.primary);
      else if (['#7DD3FC','#BAE6FD'].includes(f)) el.setAttribute('fill', c.lighter);
    }
  });

  // Update circle chart in donut phone
  document.querySelectorAll('.phone-mockup circle[stroke]').forEach(el => {
    const s = el.getAttribute('stroke');
    if (s && ['#0EA5E9','#38BDF8'].includes(s)) el.setAttribute('stroke', c.primary);
  });

  // Update chart preview cards too
  document.querySelectorAll('.chart-preview svg path[stroke="#0EA5E9"]').forEach(el => {
    el.setAttribute('stroke', c.primary);
  });
  document.querySelectorAll('.chart-preview svg rect[fill="#0EA5E9"]').forEach(el => {
    el.setAttribute('fill', c.primary);
  });

  // Update CSS custom property for accent color
  document.documentElement.style.setProperty('--primary', c.primary);
}

/* Gallery card content and code are generated from the MAUI Gallery source. */
const chartData = galleryData;
const grid = document.getElementById('chartGrid');
const cardForKey = key => grid.querySelector(`[onclick="openChart('${key}')"]`);
for (const [key, data] of Object.entries(chartData)) {
  let card = cardForKey(key);
  if (!card) {
    const previewKey = key === 'studio3d' ? '3dbar' : 'line';
    card = cardForKey(previewKey).cloneNode(true);
    card.setAttribute('onclick', `openChart('${key}')`);
    grid.appendChild(card);
  }
  card.dataset.category = data.category;
  card.querySelector('.chart-name').textContent = data.title;
  card.querySelector('.chart-desc').textContent = data.desc;
  const tags = card.querySelector('.chart-tags');
  tags.replaceChildren(...data.tags.map(tag => {
    const span = document.createElement('span');
    span.textContent = tag;
    return span;
  }));
}

/* ── Modal open/close ───────────────────────────────────── */
let activeChartKey = null;
let activeTab = 'xaml';

function openChart(key) {
  const data = chartData[key];
  if (!data) return;

  activeChartKey = key;
  activeTab = data.xaml ? 'xaml' : 'csharp';

  document.getElementById('modalTitle').textContent = data.title;
  document.getElementById('modalDesc').textContent = data.desc;
  document.getElementById('modalSourceNote').textContent = data.xaml
    ? 'XAML host and C# sample shown in the Gallery.'
    : 'This Gallery studio is implemented in C#; its complete page source is shown below.';

  const tagsEl = document.getElementById('modalTags');
  tagsEl.innerHTML = data.tags.map(t => `<span>${t}</span>`).join('');

  // Reset tabs
  document.querySelectorAll('.mtab').forEach(t => {
    const isXaml = t.dataset.tab === 'xaml';
    t.hidden = isXaml && !data.xaml;
    t.classList.toggle('active', t.dataset.tab === activeTab);
  });

  renderCode(activeTab);

  document.getElementById('chartModal').classList.add('open');
  document.body.style.overflow = 'hidden';
}

function renderCode(tab) {
  const data = chartData[activeChartKey];
  if (!data) return;
  const code = data[tab] || '// Not available for this chart type';
  highlightCode(document.getElementById('modalCodeInner'), code, tab);
}

document.querySelectorAll('code[data-language]').forEach(element => {
  highlightCode(element, element.textContent, element.dataset.language);
});

function switchCodeTab(tab, btn) {
  document.querySelectorAll('.mtab').forEach(t => t.classList.remove('active'));
  btn.classList.add('active');
  activeTab = tab;
  renderCode(tab);
}

function closeModal(e) {
  if (e.target === document.getElementById('chartModal')) closeModalBtn();
}
function closeModalBtn() {
  document.getElementById('chartModal').classList.remove('open');
  document.body.style.overflow = '';
}

document.addEventListener('keydown', e => {
  if (e.key === 'Escape') closeModalBtn();
});

function copyCode() {
  const code = document.getElementById('modalCodeInner').textContent;
  const btn = document.querySelector('.code-copy-btn');
  navigator.clipboard.writeText(code).then(() => {
    const orig = btn.innerHTML;
    btn.innerHTML = `<svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="#22C55E" stroke-width="2"><polyline points="20 6 9 17 4 12"/></svg> Copied!`;
    btn.style.color = '#22C55E';
    setTimeout(() => { btn.innerHTML = orig; btn.style.color = ''; }, 2000);
  });
}

/* ── Smooth scroll offset for fixed nav ────────────────── */
document.querySelectorAll('a[href^="#"]').forEach(a => {
  a.addEventListener('click', e => {
    const id = a.getAttribute('href').slice(1);
    const target = document.getElementById(id);
    if (!target) return;
    e.preventDefault();
    const offset = 68 + 16;
    const top = target.getBoundingClientRect().top + window.scrollY - offset;
    window.scrollTo({ top, behavior: 'smooth' });
  });
});

/* ── Animate elements on scroll ────────────────────────── */
const obs = new IntersectionObserver(entries => {
  entries.forEach(entry => {
    if (entry.isIntersecting) {
      entry.target.style.opacity = '1';
      entry.target.style.transform = 'translateY(0)';
    }
  });
}, { threshold: 0.1 });

document.querySelectorAll('.chart-card, .feature-card, .pkg-card, .step').forEach(el => {
  el.style.opacity = '0';
  el.style.transform = 'translateY(20px)';
  el.style.transition = 'opacity 0.5s ease, transform 0.5s ease';
  obs.observe(el);
});
