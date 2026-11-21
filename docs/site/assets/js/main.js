const showcaseData = {
  line: {
    title: "Cartesian Line & Spline Chart",
    code: `<charts:CartesianChart Title="Revenue vs Profit"
                       Theme="Dark"
                       Palette="Ocean"
                       HeightRequest="350">
    <charts:CartesianChart.XAxes>
        <charts:CategoryAxis />
    </charts:CartesianChart.XAxes>
    <charts:CartesianChart.YAxes>
        <charts:NumericAxis LabelFormat="C0" />
    </charts:CartesianChart.YAxes>
    <charts:CartesianChart.Series>
        <charts:LineSeries Label="Revenue"
                           ItemsSource="{Binding MonthlySales}"
                           XBinding="Month"
                           YBinding="Revenue"
                           StrokeWidth="3"
                           ShowMarkers="True" />
    </charts:CartesianChart.Series>
</charts:CartesianChart>`,
    svg: `<svg viewBox="0 0 450 280" width="100%" height="100%" xmlns="http://www.w3.org/2000/svg">
      <rect width="450" height="280" rx="12" fill="#0f172a" />
      <line x1="60" y1="50" x2="410" y2="50" stroke="#334155" stroke-dasharray="4" />
      <line x1="60" y1="110" x2="410" y2="110" stroke="#334155" stroke-dasharray="4" />
      <line x1="60" y1="170" x2="410" y2="170" stroke="#334155" stroke-dasharray="4" />
      <line x1="60" y1="230" x2="410" y2="230" stroke="#475569" stroke-width="1.5" />
      <line x1="60" y1="50" x2="60" y2="230" stroke="#475569" stroke-width="1.5" />
      <text x="35" y="55" fill="#94a3b8" font-size="11" font-family="sans-serif">$150k</text>
      <text x="35" y="115" fill="#94a3b8" font-size="11" font-family="sans-serif">$100k</text>
      <text x="42" y="175" fill="#94a3b8" font-size="11" font-family="sans-serif">$50k</text>
      <text x="50" y="235" fill="#94a3b8" font-size="11" font-family="sans-serif">$0</text>
      <!-- Area Fill -->
      <path d="M 90 190 L 140 140 L 200 160 L 260 90 L 320 110 L 380 60 L 380 230 L 90 230 Z" fill="url(#blueGradient)" opacity="0.3" />
      <!-- Line -->
      <path d="M 90 190 L 140 140 L 200 160 L 260 90 L 320 110 L 380 60" fill="none" stroke="#3b82f6" stroke-width="3" />
      <!-- Markers -->
      <circle cx="90" cy="190" r="5" fill="#3b82f6" stroke="#fff" stroke-width="2" />
      <circle cx="140" cy="140" r="5" fill="#3b82f6" stroke="#fff" stroke-width="2" />
      <circle cx="200" cy="160" r="5" fill="#3b82f6" stroke="#fff" stroke-width="2" />
      <circle cx="260" cy="90" r="5" fill="#3b82f6" stroke="#fff" stroke-width="2" />
      <circle cx="320" cy="110" r="5" fill="#3b82f6" stroke="#fff" stroke-width="2" />
      <circle cx="380" cy="60" r="5" fill="#3b82f6" stroke="#fff" stroke-width="2" />
      <defs>
        <linearGradient id="blueGradient" x1="0" y1="0" x2="0" y2="1">
          <stop offset="0%" stop-color="#3b82f6" />
          <stop offset="100%" stop-color="#3b82f6" stop-opacity="0" />
        </linearGradient>
      </defs>
    </svg>`
  },
  bar: {
    title: "3D Cuboid & Column Series",
    code: `<charts:ThreeDChart CameraPreset="Perspective"
                    IsRotationEnabled="True">
    <charts:ThreeDChart.Series>
        <charts:Column3DSeries ItemsSource="{Binding RegionalSales}"
                               XBinding="Region"
                               YBinding="Quarter"
                               ZBinding="Revenue"
                               Color="#3b82f6" />
    </charts:ThreeDChart.Series>
</charts:ThreeDChart>`,
    svg: `<svg viewBox="0 0 450 280" width="100%" height="100%" xmlns="http://www.w3.org/2000/svg">
      <rect width="450" height="280" rx="12" fill="#0f172a" />
      <!-- Isometric / 3D Columns -->
      <!-- Column 1 -->
      <polygon points="100,210 140,225 140,150 100,135" fill="#2563eb" />
      <polygon points="140,225 170,205 170,130 140,150" fill="#1d4ed8" />
      <polygon points="100,135 140,150 170,130 130,115" fill="#60a5fa" />
      <!-- Column 2 -->
      <polygon points="190,210 230,225 230,110 190,95" fill="#059669" />
      <polygon points="230,225 260,205 260,90 230,110" fill="#047857" />
      <polygon points="190,95 230,110 260,90 220,75" fill="#34d399" />
      <!-- Column 3 -->
      <polygon points="280,210 320,225 320,70 280,55" fill="#7c3aed" />
      <polygon points="320,225 350,205 350,50 320,70" fill="#6d28d9" />
      <polygon points="280,55 320,70 350,50 310,35" fill="#a78bfa" />
    </svg>`
  },
  pie: {
    title: "Donut & Pie Slices with Hole Radius",
    code: `<charts:PieChart Title="Traffic Sources"
                 HoleRadiusRatio="0.55"
                 Theme="Dark">
    <charts:PieChart.Slices>
        <charts:PieSlice Label="Direct" Value="45" Color="#3b82f6" />
        <charts:PieSlice Label="Referral" Value="30" Color="#10b981" />
        <charts:PieSlice Label="Social" Value="25" Color="#8b5cf6" />
    </charts:PieChart.Slices>
</charts:PieChart>`,
    svg: `<svg viewBox="0 0 450 280" width="100%" height="100%" xmlns="http://www.w3.org/2000/svg">
      <rect width="450" height="280" rx="12" fill="#0f172a" />
      <circle cx="225" cy="140" r="85" fill="none" stroke="#3b82f6" stroke-width="45" stroke-dasharray="240 500" stroke-dashoffset="0" />
      <circle cx="225" cy="140" r="85" fill="none" stroke="#10b981" stroke-width="45" stroke-dasharray="160 500" stroke-dashoffset="-240" />
      <circle cx="225" cy="140" r="85" fill="none" stroke="#8b5cf6" stroke-width="45" stroke-dasharray="134 500" stroke-dashoffset="-400" />
      <text x="225" y="135" text-anchor="middle" fill="#f8fafc" font-size="20" font-weight="bold" font-family="sans-serif">100%</text>
      <text x="225" y="155" text-anchor="middle" fill="#94a3b8" font-size="12" font-family="sans-serif">Total Traffic</text>
    </svg>`
  },
  financial: {
    title: "Candlestick & Technical Indicators (SMA/EMA)",
    code: `<charts:FinancialChart Title="MSFT - Daily"
                       ShowVolume="True">
    <charts:CandlestickSeries ItemsSource="{Binding Candlesticks}"
                             BullishColor="#10b981"
                             BearishColor="#ef4444" />
    <charts:SmaIndicator Period="14" Color="#f59e0b" StrokeWidth="2" />
</charts:FinancialChart>`,
    svg: `<svg viewBox="0 0 450 280" width="100%" height="100%" xmlns="http://www.w3.org/2000/svg">
      <rect width="450" height="280" rx="12" fill="#0f172a" />
      <!-- Grid -->
      <line x1="50" y1="50" x2="410" y2="50" stroke="#334155" stroke-dasharray="4" />
      <line x1="50" y1="120" x2="410" y2="120" stroke="#334155" stroke-dasharray="4" />
      <line x1="50" y1="190" x2="410" y2="190" stroke="#334155" stroke-dasharray="4" />
      <!-- Candle 1 (Green) -->
      <line x1="90" y1="110" x2="90" y2="180" stroke="#10b981" stroke-width="2" />
      <rect x="80" y="130" width="20" height="40" rx="2" fill="#10b981" />
      <!-- Candle 2 (Red) -->
      <line x1="150" y1="90" x2="150" y2="190" stroke="#ef4444" stroke-width="2" />
      <rect x="140" y="110" width="20" height="60" rx="2" fill="#ef4444" />
      <!-- Candle 3 (Green) -->
      <line x1="210" y1="80" x2="210" y2="160" stroke="#10b981" stroke-width="2" />
      <rect x="200" y="90" width="20" height="50" rx="2" fill="#10b981" />
      <!-- Candle 4 (Green) -->
      <line x1="270" y1="50" x2="270" y2="130" stroke="#10b981" stroke-width="2" />
      <rect x="260" y="60" width="20" height="40" rx="2" fill="#10b981" />
      <!-- SMA Curve -->
      <path d="M 90 150 Q 150 140 210 115 T 330 80" fill="none" stroke="#f59e0b" stroke-width="2.5" />
    </svg>`
  },
  gauge: {
    title: "Radial & Circular Progress Gauge",
    code: `<gauges:RadialGauge Value="78"
                     Minimum="0"
                     Maximum="100"
                     Unit="%"
                     TrackColor="#334155"
                     ValueColor="#3b82f6"
                     Theme="Dark" />`,
    svg: `<svg viewBox="0 0 450 280" width="100%" height="100%" xmlns="http://www.w3.org/2000/svg">
      <rect width="450" height="280" rx="12" fill="#0f172a" />
      <path d="M 125 210 A 110 110 0 1 1 325 210" fill="none" stroke="#334155" stroke-width="22" stroke-linecap="round" />
      <path d="M 125 210 A 110 110 0 1 1 300 110" fill="none" stroke="#3b82f6" stroke-width="22" stroke-linecap="round" />
      <text x="225" y="160" text-anchor="middle" fill="#f8fafc" font-size="34" font-weight="bold" font-family="sans-serif">78%</text>
      <text x="225" y="190" text-anchor="middle" fill="#94a3b8" font-size="14" font-family="sans-serif">System Health</text>
    </svg>`
  }
};

function switchTab(type) {
  document.querySelectorAll('.tab-btn').forEach(btn => btn.classList.remove('active'));
  const activeBtn = document.querySelector(`[data-tab="${type}"]`);
  if (activeBtn) activeBtn.classList.add('active');

  const data = showcaseData[type];
  if (!data) return;

  document.getElementById('showcase-svg').innerHTML = data.svg;
  document.getElementById('showcase-code').textContent = data.code;
  document.getElementById('showcase-title').textContent = data.title;
}

function copyInstall() {
  const text = "dotnet add package ChartNivra.Maui";
  navigator.clipboard.writeText(text).then(() => {
    const btn = document.querySelector('.copy-btn');
    btn.textContent = "Copied!";
    setTimeout(() => { btn.textContent = "Copy"; }, 2000);
  });
}

document.addEventListener('DOMContentLoaded', () => {
  switchTab('line');
});
