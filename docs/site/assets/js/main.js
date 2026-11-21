const suiteCatalog = {
  // 1. Line & Spline
  line: {
    category: "Cartesian 2D",
    name: "Multi-Series Spline Chart",
    description: "Cubic Hermite & Catmull-Rom spline curves with customizable data points, hover markers, and crisp anti-aliasing.",
    code: `<charts:CartesianChart Title="Weekly Activity"
                       Theme="CleanLight"
                       HeightRequest="300">
    <charts:CartesianChart.XAxes>
        <charts:CategoryAxis />
    </charts:CartesianChart.XAxes>
    <charts:CartesianChart.YAxes>
        <charts:NumericAxis />
    </charts:CartesianChart.YAxes>
    <charts:CartesianChart.Series>
        <charts:LineSeries Label="Steps"
                           ItemsSource="{Binding Steps}"
                           Color="#00C096"
                           StrokeWidth="3.5"
                           ShowMarkers="True" />
        <charts:LineSeries Label="Active Min"
                           ItemsSource="{Binding ActiveMins}"
                           Color="#3B82F6"
                           StrokeWidth="3.0"
                           ShowMarkers="True" />
    </charts:CartesianChart.Series>
</charts:CartesianChart>`,
    svg: `<svg viewBox="0 0 500 280" width="100%" height="100%" xmlns="http://www.w3.org/2000/svg">
      <line x1="50" y1="40" x2="460" y2="40" stroke="#F1F5F9" stroke-width="1.2" />
      <line x1="50" y1="95" x2="460" y2="95" stroke="#F1F5F9" stroke-width="1.2" />
      <line x1="50" y1="150" x2="460" y2="150" stroke="#F1F5F9" stroke-width="1.2" />
      <line x1="50" y1="205" x2="460" y2="205" stroke="#E2E8F0" stroke-width="1.5" />
      <text x="30" y="45" fill="#94A3B8" font-size="11" font-weight="600" font-family="'Plus Jakarta Sans', sans-serif">15k</text>
      <text x="30" y="100" fill="#94A3B8" font-size="11" font-weight="600" font-family="'Plus Jakarta Sans', sans-serif">10k</text>
      <text x="35" y="155" fill="#94A3B8" font-size="11" font-weight="600" font-family="'Plus Jakarta Sans', sans-serif">5k</text>
      <text x="40" y="210" fill="#94A3B8" font-size="11" font-weight="600" font-family="'Plus Jakarta Sans', sans-serif">0</text>
      <!-- Teal Spline Curve with Area Glow -->
      <path d="M 80 145 C 130 110, 160 160, 210 95 C 260 40, 310 120, 360 70 C 400 35, 420 60, 440 50 L 440 205 L 80 205 Z" fill="url(#tealFade)" opacity="0.12" />
      <path d="M 80 145 C 130 110, 160 160, 210 95 C 260 40, 310 120, 360 70 C 400 35, 420 60, 440 50" fill="none" stroke="#00C096" stroke-width="3.5" stroke-linecap="round" />
      <circle cx="210" cy="95" r="5" fill="#00C096" stroke="#FFFFFF" stroke-width="2.5" />
      <circle cx="360" cy="70" r="5" fill="#00C096" stroke="#FFFFFF" stroke-width="2.5" />
      <!-- Blue Line Curve -->
      <path d="M 80 175 C 130 145, 160 170, 210 135 C 260 110, 310 145, 360 115 C 400 90, 420 110, 440 95" fill="none" stroke="#3B82F6" stroke-width="3.0" stroke-linecap="round" />
      <text x="80" y="228" text-anchor="middle" fill="#64748B" font-size="11" font-weight="600" font-family="'Plus Jakarta Sans', sans-serif">Mon</text>
      <text x="170" y="228" text-anchor="middle" fill="#64748B" font-size="11" font-weight="600" font-family="'Plus Jakarta Sans', sans-serif">Tue</text>
      <text x="260" y="228" text-anchor="middle" fill="#64748B" font-size="11" font-weight="600" font-family="'Plus Jakarta Sans', sans-serif">Wed</text>
      <text x="350" y="228" text-anchor="middle" fill="#64748B" font-size="11" font-weight="600" font-family="'Plus Jakarta Sans', sans-serif">Thu</text>
      <text x="440" y="228" text-anchor="middle" fill="#64748B" font-size="11" font-weight="600" font-family="'Plus Jakarta Sans', sans-serif">Fri</text>
      <defs>
        <linearGradient id="tealFade" x1="0" y1="0" x2="0" y2="1">
          <stop offset="0%" stop-color="#00C096" />
          <stop offset="100%" stop-color="#00C096" stop-opacity="0" />
        </linearGradient>
      </defs>
    </svg>`
  },

  // 2. Health & Activity Rings
  rings: {
    category: "Circular Gauges",
    name: "Activity Progress Rings",
    description: "Multi-parameter concentric or radial progress rings with bold center readouts for fitness and KPI tracking.",
    code: `<gauges:RadialGauge Value="72"
                     TrackColor="#F1F5F9"
                     ValueColor="#00C096"
                     StrokeWidth="14"
                     Title="Heart Rate" />`,
    svg: `<svg viewBox="0 0 500 280" width="100%" height="100%" xmlns="http://www.w3.org/2000/svg">
      <!-- Ring 1 (Teal) -->
      <circle cx="120" cy="130" r="54" fill="none" stroke="#F1F5F9" stroke-width="12" />
      <circle cx="120" cy="130" r="54" fill="none" stroke="#00C096" stroke-width="12" stroke-dasharray="260 340" stroke-linecap="round" />
      <text x="120" y="132" text-anchor="middle" fill="#0F172A" font-size="24" font-weight="800" font-family="'Plus Jakarta Sans', sans-serif">72</text>
      <text x="120" y="150" text-anchor="middle" fill="#64748B" font-size="11" font-weight="600" font-family="'Plus Jakarta Sans', sans-serif">bpm</text>
      <text x="120" y="215" text-anchor="middle" fill="#0F172A" font-size="13" font-weight="700" font-family="'Plus Jakarta Sans', sans-serif">Heart Rate</text>
      <!-- Ring 2 (Purple) -->
      <circle cx="250" cy="130" r="54" fill="none" stroke="#F1F5F9" stroke-width="12" />
      <circle cx="250" cy="130" r="54" fill="none" stroke="#8B5CF6" stroke-width="12" stroke-dasharray="290 340" stroke-linecap="round" />
      <text x="250" y="132" text-anchor="middle" fill="#0F172A" font-size="24" font-weight="800" font-family="'Plus Jakarta Sans', sans-serif">85</text>
      <text x="250" y="150" text-anchor="middle" fill="#64748B" font-size="11" font-weight="600" font-family="'Plus Jakarta Sans', sans-serif">Score</text>
      <text x="250" y="215" text-anchor="middle" fill="#0F172A" font-size="13" font-weight="700" font-family="'Plus Jakarta Sans', sans-serif">Sleep Quality</text>
      <!-- Ring 3 (Blue) -->
      <circle cx="380" cy="130" r="54" fill="none" stroke="#F1F5F9" stroke-width="12" />
      <circle cx="380" cy="130" r="54" fill="none" stroke="#3B82F6" stroke-width="12" stroke-dasharray="270 340" stroke-linecap="round" />
      <text x="380" y="132" text-anchor="middle" fill="#0F172A" font-size="22" font-weight="800" font-family="'Plus Jakarta Sans', sans-serif">8.4k</text>
      <text x="380" y="150" text-anchor="middle" fill="#64748B" font-size="11" font-weight="600" font-family="'Plus Jakarta Sans', sans-serif">Steps</text>
      <text x="380" y="215" text-anchor="middle" fill="#0F172A" font-size="13" font-weight="700" font-family="'Plus Jakarta Sans', sans-serif">Daily Steps</text>
    </svg>`
  },

  // 3. Segmented Category Bars
  bars: {
    category: "Cartesian 2D",
    name: "Segmented Category Bar",
    description: "Rounded horizontal time-tracking blocks with category multi-color segment assignments.",
    code: `<charts:CartesianChart Title="Tracked Hours" Theme="CleanLight">
    <charts:BarSeries ItemsSource="{Binding DailyTrack}"
                      BarWidthRatio="0.65"
                      CornerRadius="8"
                      Color="#3B82F6" />
</charts:CartesianChart>`,
    svg: `<svg viewBox="0 0 500 280" width="100%" height="100%" xmlns="http://www.w3.org/2000/svg">
      <text x="35" y="65" fill="#64748B" font-size="12" font-weight="600" font-family="'Plus Jakarta Sans', sans-serif">Mon</text>
      <rect x="75" y="50" width="70" height="22" rx="6" fill="#8B5CF6" />
      <rect x="150" y="50" width="90" height="22" rx="6" fill="#F59E0B" />
      <rect x="245" y="50" width="160" height="22" rx="6" fill="#00C096" />

      <text x="35" y="115" fill="#64748B" font-size="12" font-weight="600" font-family="'Plus Jakarta Sans', sans-serif">Tue</text>
      <rect x="75" y="100" width="120" height="22" rx="6" fill="#F59E0B" />
      <rect x="200" y="100" width="140" height="22" rx="6" fill="#8B5CF6" />

      <text x="35" y="165" fill="#64748B" font-size="12" font-weight="600" font-family="'Plus Jakarta Sans', sans-serif">Wed</text>
      <rect x="75" y="150" width="100" height="22" rx="6" fill="#F59E0B" />
      <rect x="180" y="150" width="160" height="22" rx="6" fill="#3B82F6" />

      <text x="35" y="215" fill="#64748B" font-size="12" font-weight="600" font-family="'Plus Jakarta Sans', sans-serif">Thu</text>
      <rect x="75" y="200" width="150" height="22" rx="6" fill="#8B5CF6" />
      <rect x="230" y="200" width="90" height="22" rx="6" fill="#F43F5E" />
    </svg>`
  },

  // 4. Donut & Pie Slices
  donut: {
    category: "Circular",
    name: "Donut & Proportional Slices",
    description: "High-contrast proportional distribution charts with center summary text and slice explosion.",
    code: `<charts:PieChart Title="Storage Allocation"
                 HoleRadiusRatio="0.62">
    <charts:PieChart.Slices>
        <charts:PieSlice Label="Documents" Value="45" Color="#00C096" />
        <charts:PieSlice Label="Media" Value="35" Color="#3B82F6" />
        <charts:PieSlice Label="Apps" Value="20" Color="#8B5CF6" />
    </charts:PieChart.Slices>
</charts:PieChart>`,
    svg: `<svg viewBox="0 0 500 280" width="100%" height="100%" xmlns="http://www.w3.org/2000/svg">
      <circle cx="250" cy="140" r="80" fill="none" stroke="#00C096" stroke-width="40" stroke-dasharray="230 520" />
      <circle cx="250" cy="140" r="80" fill="none" stroke="#3B82F6" stroke-width="40" stroke-dasharray="180 520" stroke-dashoffset="-230" />
      <circle cx="250" cy="140" r="80" fill="none" stroke="#8B5CF6" stroke-width="40" stroke-dasharray="110 520" stroke-dashoffset="-410" />
      <text x="250" y="136" text-anchor="middle" fill="#0F172A" font-size="22" font-weight="800" font-family="'Plus Jakarta Sans', sans-serif">100%</text>
      <text x="250" y="156" text-anchor="middle" fill="#64748B" font-size="12" font-weight="600" font-family="'Plus Jakarta Sans', sans-serif">Allocation</text>
    </svg>`
  },

  // 5. Candlestick Financial
  candlestick: {
    category: "Financial",
    name: "Candlestick with SMA Overlay",
    description: "Enterprise OHLC series displaying market open/close bodies, high/low wicks, and weighted SMA indicators.",
    code: `<charts:FinancialChart Title="MSFT Daily">
    <charts:CandlestickSeries ItemsSource="{Binding Candles}"
                             BullishColor="#00C096"
                             BearishColor="#EF4444" />
    <charts:SmaIndicator Period="14" Color="#F59E0B" StrokeWidth="2.5" />
</charts:FinancialChart>`,
    svg: `<svg viewBox="0 0 500 280" width="100%" height="100%" xmlns="http://www.w3.org/2000/svg">
      <line x1="45" y1="210" x2="455" y2="210" stroke="#E2E8F0" stroke-width="1.5" />
      <!-- Candle 1 -->
      <line x1="110" y1="90" x2="110" y2="180" stroke="#00C096" stroke-width="1.5" />
      <rect x="100" y="110" width="20" height="45" rx="3" fill="#00C096" />
      <!-- Candle 2 -->
      <line x1="190" y1="70" x2="190" y2="190" stroke="#EF4444" stroke-width="1.5" />
      <rect x="180" y="90" width="20" height="75" rx="3" fill="#EF4444" />
      <!-- Candle 3 -->
      <line x1="270" y1="60" x2="270" y2="150" stroke="#00C096" stroke-width="1.5" />
      <rect x="260" y="75" width="20" height="55" rx="3" fill="#00C096" />
      <!-- Candle 4 -->
      <line x1="350" y1="45" x2="350" y2="135" stroke="#00C096" stroke-width="1.5" />
      <rect x="340" y="55" width="20" height="50" rx="3" fill="#00C096" />
      <!-- SMA Curve -->
      <path d="M 90 140 Q 180 120 270 95 T 390 65" fill="none" stroke="#F59E0B" stroke-width="2.5" />
    </svg>`
  },

  // 6. True 3D Cuboid Columns
  threed: {
    category: "True 3D Analytical",
    name: "3D Perspective Cuboid Column",
    description: "Real three-dimensional geometry with camera view/projection matrix transforms and directional lighting shading.",
    code: `<threed:ThreeDChart CameraPreset="Perspective"
                    IsRotationEnabled="True">
    <threed:Column3DSeries ItemsSource="{Binding PerformanceMatrix}"
                          Color="#00C096" />
</threed:ThreeDChart>`,
    svg: `<svg viewBox="0 0 500 280" width="100%" height="100%" xmlns="http://www.w3.org/2000/svg">
      <polygon points="60,190 250,235 440,190 250,150" fill="#F8FAFC" stroke="#E2E8F0" stroke-width="1.2" />
      <!-- Col 1 (Teal) -->
      <polygon points="120,180 160,190 160,105 120,95" fill="#00A883" />
      <polygon points="160,190 195,175 195,90 160,105" fill="#008F6F" />
      <polygon points="120,95 160,105 195,90 155,80" fill="#34D399" />
      <!-- Col 2 (Blue) -->
      <polygon points="220,190 265,200 265,65 220,55" fill="#2563EB" />
      <polygon points="265,200 300,185 300,50 265,65" fill="#1D4ED8" />
      <polygon points="220,55 265,65 300,50 255,40" fill="#60A5FA" />
      <!-- Col 3 (Purple) -->
      <polygon points="320,180 365,190 365,85 320,75" fill="#7C3AED" />
      <polygon points="365,190 400,175 400,70 365,85" fill="#6D28D9" />
      <polygon points="320,75 365,85 400,70 355,60" fill="#A78BFA" />
    </svg>`
  }
};

function selectSuite(key) {
  document.querySelectorAll('.nav-item-btn').forEach(btn => btn.classList.remove('active'));
  const btn = document.querySelector(`[data-suite="${key}"]`);
  if (btn) btn.classList.add('active');

  const item = suiteCatalog[key];
  if (!item) return;

  document.getElementById('wb-tag').textContent = item.category;
  document.getElementById('wb-name').textContent = item.name;
  document.getElementById('wb-desc').textContent = item.description;
  document.getElementById('wb-svg').innerHTML = item.svg;
  document.getElementById('wb-code').textContent = item.code;
}

function copyCode() {
  const code = document.getElementById('wb-code').textContent;
  navigator.clipboard.writeText(code).then(() => {
    const btn = document.querySelector('.btn-code-copy');
    btn.textContent = 'Copied!';
    setTimeout(() => { btn.textContent = 'Copy Code'; }, 1800);
  });
}

function copyInstall() {
  navigator.clipboard.writeText('dotnet add package ChartNivra.Maui').then(() => {
    const btn = document.querySelector('.copy-button');
    btn.textContent = 'Copied!';
    setTimeout(() => { btn.textContent = 'Copy'; }, 1800);
  });
}

document.addEventListener('DOMContentLoaded', () => {
  selectSuite('line');
});
