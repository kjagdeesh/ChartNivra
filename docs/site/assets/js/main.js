const chartCatalog = {
  // 1. Line & Spline
  line: {
    category: "Cartesian 2D",
    name: "Line & Spline Chart",
    description: "Multi-series continuous line and cubic spline curves with custom markers and smooth interpolation.",
    code: `<charts:CartesianChart Title="Revenue & Profit"
                       Theme="CleanLight"
                       HeightRequest="300">
    <charts:CartesianChart.XAxes>
        <charts:CategoryAxis />
    </charts:CartesianChart.XAxes>
    <charts:CartesianChart.YAxes>
        <charts:NumericAxis LabelFormat="C0" />
    </charts:CartesianChart.YAxes>
    <charts:CartesianChart.Series>
        <charts:LineSeries Label="Revenue"
                           ItemsSource="{Binding MonthlySales}"
                           Color="#00C096"
                           StrokeWidth="3.5"
                           ShowMarkers="True" />
    </charts:CartesianChart.Series>
</charts:CartesianChart>`,
    svg: `<svg viewBox="0 0 440 280" width="100%" height="100%" xmlns="http://www.w3.org/2000/svg">
      <line x1="45" y1="40" x2="405" y2="40" stroke="#F1F5F9" stroke-width="1.2" />
      <line x1="45" y1="100" x2="405" y2="100" stroke="#F1F5F9" stroke-width="1.2" />
      <line x1="45" y1="160" x2="405" y2="160" stroke="#F1F5F9" stroke-width="1.2" />
      <line x1="45" y1="220" x2="405" y2="220" stroke="#E2E8F0" stroke-width="1.5" />
      <line x1="45" y1="40" x2="45" y2="220" stroke="#E2E8F0" stroke-width="1.5" />
      <text x="25" y="45" fill="#94A3B8" font-size="11" font-weight="600" font-family="sans-serif">15k</text>
      <text x="25" y="105" fill="#94A3B8" font-size="11" font-weight="600" font-family="sans-serif">10k</text>
      <text x="30" y="165" fill="#94A3B8" font-size="11" font-weight="600" font-family="sans-serif">5k</text>
      <text x="35" y="225" fill="#94A3B8" font-size="11" font-weight="600" font-family="sans-serif">0</text>
      <!-- Spline curve -->
      <path d="M 75 160 C 120 110, 160 170, 205 100 C 250 40, 310 130, 375 60" fill="none" stroke="#00C096" stroke-width="3.5" stroke-linecap="round" />
      <circle cx="75" cy="160" r="5" fill="#00C096" stroke="#fff" stroke-width="2.5" />
      <circle cx="205" cy="100" r="5" fill="#00C096" stroke="#fff" stroke-width="2.5" />
      <circle cx="375" cy="60" r="5" fill="#00C096" stroke="#fff" stroke-width="2.5" />
      <text x="75" y="242" text-anchor="middle" fill="#64748B" font-size="11" font-weight="600" font-family="sans-serif">Jan</text>
      <text x="205" y="242" text-anchor="middle" fill="#64748B" font-size="11" font-weight="600" font-family="sans-serif">Feb</text>
      <text x="375" y="242" text-anchor="middle" fill="#64748B" font-size="11" font-weight="600" font-family="sans-serif">Mar</text>
    </svg>`
  },

  // 2. Area Chart
  area: {
    category: "Cartesian 2D",
    name: "Area & Gradient Fill Chart",
    description: "Volume-emphasized area chart with smooth opacity gradient falloff underneath the trajectory line.",
    code: `<charts:CartesianChart Title="Network Bandwidth">
    <charts:CartesianChart.Series>
        <charts:AreaSeries ItemsSource="{Binding TrafficPoints}"
                           Color="#3B82F6"
                           FillOpacity="40"
                           StrokeWidth="3" />
    </charts:CartesianChart.Series>
</charts:CartesianChart>`,
    svg: `<svg viewBox="0 0 440 280" width="100%" height="100%" xmlns="http://www.w3.org/2000/svg">
      <line x1="45" y1="40" x2="405" y2="40" stroke="#F1F5F9" stroke-width="1.2" />
      <line x1="45" y1="100" x2="405" y2="100" stroke="#F1F5F9" stroke-width="1.2" />
      <line x1="45" y1="160" x2="405" y2="160" stroke="#F1F5F9" stroke-width="1.2" />
      <line x1="45" y1="220" x2="405" y2="220" stroke="#E2E8F0" stroke-width="1.5" />
      <!-- Area Polygon -->
      <path d="M 65 180 Q 145 120 225 150 T 385 70 L 385 220 L 65 220 Z" fill="url(#areaBlue)" opacity="0.35" />
      <path d="M 65 180 Q 145 120 225 150 T 385 70" fill="none" stroke="#3B82F6" stroke-width="3" stroke-linecap="round" />
      <defs>
        <linearGradient id="areaBlue" x1="0" y1="0" x2="0" y2="1">
          <stop offset="0%" stop-color="#3B82F6" />
          <stop offset="100%" stop-color="#3B82F6" stop-opacity="0" />
        </linearGradient>
      </defs>
    </svg>`
  },

  // 3. Column Chart
  column: {
    category: "Cartesian 2D",
    name: "Rounded Column Chart",
    description: "Vertical discrete category comparison with customizable bar ratios and corner radius.",
    code: `<charts:CartesianChart Title="Quarterly Revenue">
    <charts:CartesianChart.Series>
        <charts:BarSeries ItemsSource="{Binding Quarters}"
                          BarWidthRatio="0.6"
                          CornerRadius="8"
                          Color="#00C096" />
    </charts:CartesianChart.Series>
</charts:CartesianChart>`,
    svg: `<svg viewBox="0 0 440 280" width="100%" height="100%" xmlns="http://www.w3.org/2000/svg">
      <line x1="45" y1="220" x2="405" y2="220" stroke="#E2E8F0" stroke-width="1.5" />
      <rect x="75" y="110" width="45" height="110" rx="8" fill="#00C096" />
      <rect x="155" y="70" width="45" height="150" rx="8" fill="#3B82F6" />
      <rect x="235" y="90" width="45" height="130" rx="8" fill="#8B5CF6" />
      <rect x="315" y="50" width="45" height="170" rx="8" fill="#00C096" />
      <text x="97" y="242" text-anchor="middle" fill="#64748B" font-size="11" font-weight="600" font-family="sans-serif">Q1</text>
      <text x="177" y="242" text-anchor="middle" fill="#64748B" font-size="11" font-weight="600" font-family="sans-serif">Q2</text>
      <text x="257" y="242" text-anchor="middle" fill="#64748B" font-size="11" font-weight="600" font-family="sans-serif">Q3</text>
      <text x="337" y="242" text-anchor="middle" fill="#64748B" font-size="11" font-weight="600" font-family="sans-serif">Q4</text>
    </svg>`
  },

  // 4. Horizontal Bar
  bar: {
    category: "Cartesian 2D",
    name: "Segmented Horizontal Bar",
    description: "Horizontal category bars ideal for time-tracking, budget allocations, and leaderboards.",
    code: `<charts:CartesianChart Title="Tracked Hours">
    <charts:BarSeries ItemsSource="{Binding TimeEntries}"
                      BarWidthRatio="0.6"
                      CornerRadius="6"
                      Color="#8B5CF6" />
</charts:CartesianChart>`,
    svg: `<svg viewBox="0 0 440 280" width="100%" height="100%" xmlns="http://www.w3.org/2000/svg">
      <text x="40" y="70" fill="#64748B" font-size="12" font-weight="600" font-family="sans-serif">Mon</text>
      <rect x="80" y="55" width="220" height="20" rx="6" fill="#00C096" />
      <text x="40" y="125" fill="#64748B" font-size="12" font-weight="600" font-family="sans-serif">Tue</text>
      <rect x="80" y="110" width="280" height="20" rx="6" fill="#3B82F6" />
      <text x="40" y="180" fill="#64748B" font-size="12" font-weight="600" font-family="sans-serif">Wed</text>
      <rect x="80" y="165" width="170" height="20" rx="6" fill="#8B5CF6" />
      <text x="40" y="235" fill="#64748B" font-size="12" font-weight="600" font-family="sans-serif">Thu</text>
      <rect x="80" y="220" width="250" height="20" rx="6" fill="#F59E0B" />
    </svg>`
  },

  // 5. Scatter Plot
  scatter: {
    category: "Cartesian 2D",
    name: "Scatter & Bubble Plot",
    description: "Multi-dimensional distribution plot with variable point radii and spatial clustering.",
    code: `<charts:CartesianChart Title="Patient Metric Cluster">
    <charts:CartesianChart.Series>
        <charts:ScatterSeries ItemsSource="{Binding Vitals}"
                              DefaultPointRadius="6"
                              Color="#3B82F6" />
    </charts:CartesianChart.Series>
</charts:CartesianChart>`,
    svg: `<svg viewBox="0 0 440 280" width="100%" height="100%" xmlns="http://www.w3.org/2000/svg">
      <line x1="45" y1="40" x2="405" y2="40" stroke="#F1F5F9" stroke-width="1.2" />
      <line x1="45" y1="220" x2="405" y2="220" stroke="#E2E8F0" stroke-width="1.5" />
      <line x1="45" y1="40" x2="45" y2="220" stroke="#E2E8F0" stroke-width="1.5" />
      <circle cx="95" cy="170" r="7" fill="#3B82F6" opacity="0.8" />
      <circle cx="140" cy="130" r="11" fill="#00C096" opacity="0.8" />
      <circle cx="180" cy="180" r="6" fill="#3B82F6" opacity="0.8" />
      <circle cx="230" cy="90" r="14" fill="#8B5CF6" opacity="0.8" />
      <circle cx="280" cy="140" r="8" fill="#3B82F6" opacity="0.8" />
      <circle cx="330" cy="70" r="10" fill="#00C096" opacity="0.8" />
      <circle cx="370" cy="110" r="5" fill="#F59E0B" opacity="0.8" />
    </svg>`
  },

  // 6. Donut & Pie
  donut: {
    category: "Circular",
    name: "Donut & Pie Slices",
    description: "Proportional distribution slices with configurable center hole ratio and slice explosion.",
    code: `<charts:PieChart Title="Storage Breakdown"
                 HoleRadiusRatio="0.6">
    <charts:PieChart.Slices>
        <charts:PieSlice Label="Documents" Value="45" Color="#00C096" />
        <charts:PieSlice Label="Media" Value="35" Color="#3B82F6" />
        <charts:PieSlice Label="Apps" Value="20" Color="#8B5CF6" />
    </charts:PieChart.Slices>
</charts:PieChart>`,
    svg: `<svg viewBox="0 0 440 280" width="100%" height="100%" xmlns="http://www.w3.org/2000/svg">
      <circle cx="220" cy="140" r="75" fill="none" stroke="#00C096" stroke-width="38" stroke-dasharray="210 500" />
      <circle cx="220" cy="140" r="75" fill="none" stroke="#3B82F6" stroke-width="38" stroke-dasharray="165 500" stroke-dashoffset="-210" />
      <circle cx="220" cy="140" r="75" fill="none" stroke="#8B5CF6" stroke-width="38" stroke-dasharray="96 500" stroke-dashoffset="-375" />
      <text x="220" y="136" text-anchor="middle" fill="#0F172A" font-size="20" font-weight="800" font-family="sans-serif">100%</text>
      <text x="220" y="154" text-anchor="middle" fill="#64748B" font-size="11" font-weight="600" font-family="sans-serif">Allocated</text>
    </svg>`
  },

  // 7. Radial Gauge
  gauge: {
    category: "Gauges",
    name: "Radial & Activity Gauge",
    description: "Clean circular progress ring with center readouts and customizable warning thresholds.",
    code: `<gauges:RadialGauge Value="72"
                     Minimum="0"
                     Maximum="100"
                     Unit="bpm"
                     ValueColor="#00C096"
                     TrackColor="#F1F5F9"
                     StrokeWidth="16" />`,
    svg: `<svg viewBox="0 0 440 280" width="100%" height="100%" xmlns="http://www.w3.org/2000/svg">
      <circle cx="220" cy="140" r="75" fill="none" stroke="#F1F5F9" stroke-width="16" />
      <circle cx="220" cy="140" r="75" fill="none" stroke="#00C096" stroke-width="16" stroke-dasharray="340 480" stroke-linecap="round" />
      <text x="220" y="138" text-anchor="middle" fill="#0F172A" font-size="32" font-weight="800" font-family="sans-serif">72</text>
      <text x="220" y="162" text-anchor="middle" fill="#64748B" font-size="13" font-weight="600" font-family="sans-serif">bpm</text>
    </svg>`
  },

  // 8. Candlestick
  candlestick: {
    category: "Financial",
    name: "Candlestick & SMA Indicator",
    description: "High-precision financial OHLC series with moving average trend overlay calculations.",
    code: `<charts:FinancialChart Title="MSFT Daily">
    <charts:CandlestickSeries ItemsSource="{Binding Tickers}"
                             BullishColor="#00C096"
                             BearishColor="#EF4444" />
    <charts:SmaIndicator Period="14" Color="#F59E0B" StrokeWidth="2.5" />
</charts:FinancialChart>`,
    svg: `<svg viewBox="0 0 440 280" width="100%" height="100%" xmlns="http://www.w3.org/2000/svg">
      <line x1="45" y1="220" x2="405" y2="220" stroke="#E2E8F0" stroke-width="1.5" />
      <!-- Candle 1 -->
      <line x1="100" y1="100" x2="100" y2="180" stroke="#00C096" stroke-width="1.5" />
      <rect x="90" y="120" width="20" height="40" rx="3" fill="#00C096" />
      <!-- Candle 2 -->
      <line x1="170" y1="80" x2="170" y2="190" stroke="#EF4444" stroke-width="1.5" />
      <rect x="160" y="100" width="20" height="65" rx="3" fill="#EF4444" />
      <!-- Candle 3 -->
      <line x1="240" y1="70" x2="240" y2="150" stroke="#00C096" stroke-width="1.5" />
      <rect x="230" y="80" width="20" height="50" rx="3" fill="#00C096" />
      <!-- Candle 4 -->
      <line x1="310" y1="50" x2="310" y2="130" stroke="#00C096" stroke-width="1.5" />
      <rect x="300" y="60" width="20" height="45" rx="3" fill="#00C096" />
      <!-- SMA Curve -->
      <path d="M 90 145 Q 165 130 240 105 T 330 75" fill="none" stroke="#F59E0B" stroke-width="2.5" />
    </svg>`
  },

  // 9. Statistical Box Plot
  boxplot: {
    category: "Statistical",
    name: "Box & Whisker Plot",
    description: "Five-number statistical summary displaying Min, Q1, Median, Q3, and Max ranges.",
    code: `<charts:StatisticalChart Title="Sample Variance">
    <charts:BoxPlotSeries ItemsSource="{Binding Summaries}"
                          BoxColor="#3B82F6"
                          WhiskerColor="#0F172A" />
</charts:StatisticalChart>`,
    svg: `<svg viewBox="0 0 440 280" width="100%" height="100%" xmlns="http://www.w3.org/2000/svg">
      <line x1="45" y1="220" x2="405" y2="220" stroke="#E2E8F0" stroke-width="1.5" />
      <!-- Box 1 -->
      <line x1="130" y1="60" x2="130" y2="190" stroke="#0F172A" stroke-width="1.5" />
      <rect x="105" y="90" width="50" height="70" rx="4" fill="#3B82F6" />
      <line x1="105" y1="120" x2="155" y2="120" stroke="#FFFFFF" stroke-width="2" />
      <!-- Box 2 -->
      <line x1="270" y1="40" x2="270" y2="200" stroke="#0F172A" stroke-width="1.5" />
      <rect x="245" y="75" width="50" height="85" rx="4" fill="#00C096" />
      <line x1="245" y1="110" x2="295" y2="110" stroke="#FFFFFF" stroke-width="2" />
    </svg>`
  },

  // 10. True 3D Column
  threed: {
    category: "True 3D Analytical",
    name: "3D Perspective Cuboid Column",
    description: "Actual 3D mesh geometry with Camera3D projection, depth sorting, and Lambertian lighting.",
    code: `<threed:ThreeDChart CameraPreset="Perspective"
                    IsRotationEnabled="True">
    <threed:Column3DSeries ItemsSource="{Binding Matrix}"
                          Color="#00C096" />
</threed:ThreeDChart>`,
    svg: `<svg viewBox="0 0 440 280" width="100%" height="100%" xmlns="http://www.w3.org/2000/svg">
      <polygon points="50,190 220,230 390,190 220,155" fill="#F8FAFC" stroke="#E2E8F0" stroke-width="1.2" />
      <!-- Col 1 -->
      <polygon points="100,180 140,190 140,110 100,100" fill="#00A883" />
      <polygon points="140,190 170,175 170,95 140,110" fill="#008F6F" />
      <polygon points="100,100 140,110 170,95 130,85" fill="#34D399" />
      <!-- Col 2 -->
      <polygon points="190,190 230,200 230,70 190,60" fill="#2563EB" />
      <polygon points="230,200 260,185 260,55 230,70" fill="#1D4ED8" />
      <polygon points="190,60 230,70 260,55 220,45" fill="#60A5FA" />
      <!-- Col 3 -->
      <polygon points="280,180 320,190 320,90 280,80" fill="#7C3AED" />
      <polygon points="320,190 350,175 350,75 320,90" fill="#6D28D9" />
      <polygon points="280,80 320,90 350,75 310,65" fill="#A78BFA" />
    </svg>`
  }
};

function selectChart(key) {
  document.querySelectorAll('.chart-item-btn').forEach(btn => btn.classList.remove('active'));
  const targetBtn = document.querySelector(`[data-key="${key}"]`);
  if (targetBtn) targetBtn.classList.add('active');

  const item = chartCatalog[key];
  if (!item) return;

  document.getElementById('stage-category').textContent = item.category;
  document.getElementById('stage-name').textContent = item.name;
  document.getElementById('stage-desc').textContent = item.description;
  document.getElementById('stage-svg').innerHTML = item.svg;
  document.getElementById('code-display').textContent = item.code;
}

function copyCode() {
  const code = document.getElementById('code-display').textContent;
  navigator.clipboard.writeText(code).then(() => {
    const btn = document.querySelector('.btn-code-copy');
    btn.textContent = 'Copied!';
    setTimeout(() => { btn.textContent = 'Copy Code'; }, 1800);
  });
}

function copyInstall() {
  navigator.clipboard.writeText('dotnet add package ChartNivra.Maui').then(() => {
    const btn = document.querySelector('.copy-badge');
    btn.textContent = 'Copied!';
    setTimeout(() => { btn.textContent = 'Copy'; }, 1800);
  });
}

document.addEventListener('DOMContentLoaded', () => {
  selectChart('line');
});
