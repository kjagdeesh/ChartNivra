const showcaseData = {
  line: {
    title: "Weekly Activity & Progress (Multi-Line)",
    code: `<charts:CartesianChart Title="Weekly Progress"
                       Theme="CleanLight"
                       Palette="Clean"
                       HeightRequest="320">
    <charts:CartesianChart.XAxes>
        <charts:CategoryAxis />
    </charts:CartesianChart.XAxes>
    <charts:CartesianChart.YAxes>
        <charts:NumericAxis />
    </charts:CartesianChart.YAxes>
    <charts:CartesianChart.Series>
        <charts:LineSeries Label="Steps"
                           ItemsSource="{Binding StepsData}"
                           Color="#00C096"
                           StrokeWidth="3.5"
                           ShowMarkers="True" />
        <charts:LineSeries Label="Active Min"
                           ItemsSource="{Binding ActiveData}"
                           Color="#3B82F6"
                           StrokeWidth="3"
                           ShowMarkers="True" />
        <charts:LineSeries Label="Calories"
                           ItemsSource="{Binding CalorieData}"
                           Color="#8B5CF6"
                           StrokeWidth="2.5"
                           ShowMarkers="True" />
    </charts:CartesianChart.Series>
</charts:CartesianChart>`,
    svg: `<svg viewBox="0 0 460 290" width="100%" height="100%" xmlns="http://www.w3.org/2000/svg">
      <rect width="460" height="290" rx="16" fill="#FFFFFF" />
      <!-- Grid lines -->
      <line x1="55" y1="45" x2="425" y2="45" stroke="#F1F5F9" stroke-width="1.2" />
      <line x1="55" y1="105" x2="425" y2="105" stroke="#F1F5F9" stroke-width="1.2" />
      <line x1="55" y1="165" x2="425" y2="165" stroke="#F1F5F9" stroke-width="1.2" />
      <line x1="55" y1="225" x2="425" y2="225" stroke="#E2E8F0" stroke-width="1.5" />
      <line x1="55" y1="45" x2="55" y2="225" stroke="#E2E8F0" stroke-width="1.5" />
      <!-- Y Labels -->
      <text x="30" y="50" fill="#94A3B8" font-size="11" font-weight="600" font-family="sans-serif">15k</text>
      <text x="30" y="110" fill="#94A3B8" font-size="11" font-weight="600" font-family="sans-serif">10k</text>
      <text x="35" y="170" fill="#94A3B8" font-size="11" font-weight="600" font-family="sans-serif">5k</text>
      <text x="40" y="230" fill="#94A3B8" font-size="11" font-weight="600" font-family="sans-serif">0</text>
      <!-- X Labels -->
      <text x="90" y="246" text-anchor="middle" fill="#64748B" font-size="11" font-weight="600" font-family="sans-serif">Mon</text>
      <text x="145" y="246" text-anchor="middle" fill="#64748B" font-size="11" font-weight="600" font-family="sans-serif">Tue</text>
      <text x="200" y="246" text-anchor="middle" fill="#64748B" font-size="11" font-weight="600" font-family="sans-serif">Wed</text>
      <text x="255" y="246" text-anchor="middle" fill="#64748B" font-size="11" font-weight="600" font-family="sans-serif">Thu</text>
      <text x="310" y="246" text-anchor="middle" fill="#64748B" font-size="11" font-weight="600" font-family="sans-serif">Fri</text>
      <text x="365" y="246" text-anchor="middle" fill="#64748B" font-size="11" font-weight="600" font-family="sans-serif">Sat</text>
      <text x="415" y="246" text-anchor="middle" fill="#64748B" font-size="11" font-weight="600" font-family="sans-serif">Sun</text>
      <!-- Area Fill for Series 1 (Teal) -->
      <path d="M 90 145 C 120 120, 135 150, 145 130 C 160 100, 185 110, 200 95 C 220 75, 240 85, 255 105 C 275 135, 295 90, 310 80 C 335 65, 350 85, 365 70 C 390 50, 405 60, 415 50 L 415 225 L 90 225 Z" fill="url(#tealGlow)" opacity="0.15" />
      <!-- Line 1: Steps (Teal #00C096) -->
      <path d="M 90 145 C 120 120, 135 150, 145 130 C 160 100, 185 110, 200 95 C 220 75, 240 85, 255 105 C 275 135, 295 90, 310 80 C 335 65, 350 85, 365 70 C 390 50, 405 60, 415 50" fill="none" stroke="#00C096" stroke-width="3.5" stroke-linecap="round" />
      <circle cx="200" cy="95" r="5" fill="#00C096" stroke="#FFFFFF" stroke-width="2.5" />
      <circle cx="310" cy="80" r="5" fill="#00C096" stroke="#FFFFFF" stroke-width="2.5" />
      <circle cx="415" cy="50" r="5" fill="#00C096" stroke="#FFFFFF" stroke-width="2.5" />
      <!-- Line 2: Active (Blue #3B82F6) -->
      <path d="M 90 170 C 120 150, 135 165, 145 155 C 160 135, 185 145, 200 130 C 220 115, 240 120, 255 135 C 275 160, 295 125, 310 115 C 335 105, 350 120, 365 105 C 390 90, 405 95, 415 85" fill="none" stroke="#3B82F6" stroke-width="3" stroke-linecap="round" />
      <!-- Line 3: Calories (Purple #8B5CF6) -->
      <path d="M 90 195 C 120 185, 135 190, 145 180 C 160 165, 185 175, 200 160 C 220 145, 240 155, 255 170 C 275 190, 295 160, 310 150 C 335 140, 350 155, 365 140 C 390 125, 405 130, 415 120" fill="none" stroke="#8B5CF6" stroke-width="2.5" stroke-linecap="round" />
      <defs>
        <linearGradient id="tealGlow" x1="0" y1="0" x2="0" y2="1">
          <stop offset="0%" stop-color="#00C096" />
          <stop offset="100%" stop-color="#00C096" stop-opacity="0" />
        </linearGradient>
      </defs>
    </svg>`
  },
  rings: {
    title: "Health & Activity Circular Rings",
    code: `<gauges:RadialGauge Value="72"
                     TrackColor="#F1F5F9"
                     ValueColor="#00C096"
                     StrokeWidth="14"
                     Title="Heart Rate" />`,
    svg: `<svg viewBox="0 0 460 290" width="100%" height="100%" xmlns="http://www.w3.org/2000/svg">
      <rect width="460" height="290" rx="16" fill="#FFFFFF" />
      <!-- Ring 1 (Teal) -->
      <circle cx="110" cy="130" r="50" fill="none" stroke="#F1F5F9" stroke-width="12" />
      <circle cx="110" cy="130" r="50" fill="none" stroke="#00C096" stroke-width="12" stroke-dasharray="240 320" stroke-linecap="round" />
      <text x="110" y="132" text-anchor="middle" fill="#0F172A" font-size="22" font-weight="bold" font-family="sans-serif">72</text>
      <text x="110" y="148" text-anchor="middle" fill="#64748B" font-size="11" font-family="sans-serif">bpm</text>
      <text x="110" y="210" text-anchor="middle" fill="#0F172A" font-size="13" font-weight="bold" font-family="sans-serif">Heart Rate</text>
      <!-- Ring 2 (Purple) -->
      <circle cx="230" cy="130" r="50" fill="none" stroke="#F1F5F9" stroke-width="12" />
      <circle cx="230" cy="130" r="50" fill="none" stroke="#8B5CF6" stroke-width="12" stroke-dasharray="270 320" stroke-linecap="round" />
      <text x="230" y="132" text-anchor="middle" fill="#0F172A" font-size="22" font-weight="bold" font-family="sans-serif">85</text>
      <text x="230" y="148" text-anchor="middle" fill="#64748B" font-size="11" font-family="sans-serif">score</text>
      <text x="230" y="210" text-anchor="middle" fill="#0F172A" font-size="13" font-weight="bold" font-family="sans-serif">Sleep Quality</text>
      <!-- Ring 3 (Blue) -->
      <circle cx="350" cy="130" r="50" fill="none" stroke="#F1F5F9" stroke-width="12" />
      <circle cx="350" cy="130" r="50" fill="none" stroke="#3B82F6" stroke-width="12" stroke-dasharray="250 320" stroke-linecap="round" />
      <text x="350" y="132" text-anchor="middle" fill="#0F172A" font-size="20" font-weight="bold" font-family="sans-serif">8.4k</text>
      <text x="350" y="148" text-anchor="middle" fill="#64748B" font-size="11" font-family="sans-serif">steps</text>
      <text x="350" y="210" text-anchor="middle" fill="#0F172A" font-size="13" font-weight="bold" font-family="sans-serif">Daily Steps</text>
    </svg>`
  },
  columns: {
    title: "Segmented Bar & Category Hours",
    code: `<charts:CartesianChart Title="Tracked Hours"
                       Theme="CleanLight">
    <charts:BarSeries ItemsSource="{Binding TimeEntries}"
                      BarWidthRatio="0.65"
                      CornerRadius="8"
                      Color="#3B82F6" />
</charts:CartesianChart>`,
    svg: `<svg viewBox="0 0 460 290" width="100%" height="100%" xmlns="http://www.w3.org/2000/svg">
      <rect width="460" height="290" rx="16" fill="#FFFFFF" />
      <!-- Horizontal Bars -->
      <text x="40" y="65" fill="#64748B" font-size="12" font-weight="600" font-family="sans-serif">Mon</text>
      <rect x="80" y="50" width="60" height="22" rx="6" fill="#8B5CF6" />
      <rect x="145" y="50" width="80" height="22" rx="6" fill="#F59E0B" />
      <rect x="230" y="50" width="120" height="22" rx="6" fill="#00C096" />

      <text x="40" y="110" fill="#64748B" font-size="12" font-weight="600" font-family="sans-serif">Tue</text>
      <rect x="80" y="95" width="110" height="22" rx="6" fill="#F59E0B" />
      <rect x="195" y="95" width="95" height="22" rx="6" fill="#8B5CF6" />

      <text x="40" y="155" fill="#64748B" font-size="12" font-weight="600" font-family="sans-serif">Wed</text>
      <rect x="80" y="140" width="90" height="22" rx="6" fill="#F59E0B" />
      <rect x="175" y="140" width="110" height="22" rx="6" fill="#3B82F6" />

      <text x="40" y="200" fill="#64748B" font-size="12" font-weight="600" font-family="sans-serif">Thu</text>
      <rect x="80" y="185" width="130" height="22" rx="6" fill="#8B5CF6" />
      <rect x="215" y="185" width="75" height="22" rx="6" fill="#F43F5E" />

      <text x="40" y="245" fill="#64748B" font-size="12" font-weight="600" font-family="sans-serif">Fri</text>
      <rect x="80" y="230" width="160" height="22" rx="6" fill="#3B82F6" />
      <rect x="245" y="230" width="110" height="22" rx="6" fill="#00C096" />
    </svg>`
  },
  threed: {
    title: "True 3D Cuboids with Lambertian Shading",
    code: `<threed:ThreeDChart CameraPreset="Perspective"
                    IsRotationEnabled="True">
    <threed:Column3DSeries ItemsSource="{Binding Analytics}"
                          Color="#00C096" />
</threed:ThreeDChart>`,
    svg: `<svg viewBox="0 0 460 290" width="100%" height="100%" xmlns="http://www.w3.org/2000/svg">
      <rect width="460" height="290" rx="16" fill="#FFFFFF" />
      <!-- Grid Floor -->
      <polygon points="60,200 230,240 400,200 230,165" fill="#F8FAFC" stroke="#E2E8F0" stroke-width="1.2" />
      <!-- 3D Column 1 (Teal) -->
      <polygon points="110,185 150,195 150,115 110,105" fill="#00A883" />
      <polygon points="150,195 180,180 180,100 150,115" fill="#008F6F" />
      <polygon points="110,105 150,115 180,100 140,90" fill="#34D399" />
      <!-- 3D Column 2 (Blue) -->
      <polygon points="200,195 240,205 240,75 200,65" fill="#2563EB" />
      <polygon points="240,205 270,190 270,60 240,75" fill="#1D4ED8" />
      <polygon points="200,65 240,75 270,60 230,50" fill="#60A5FA" />
      <!-- 3D Column 3 (Purple) -->
      <polygon points="290,185 330,195 330,95 290,85" fill="#7C3AED" />
      <polygon points="330,195 360,180 360,80 330,95" fill="#6D28D9" />
      <polygon points="290,85 330,95 360,80 320,70" fill="#A78BFA" />
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
    const btn = document.querySelector('.btn-copy');
    btn.textContent = "Copied!";
    setTimeout(() => { btn.textContent = "Copy"; }, 2000);
  });
}

document.addEventListener('DOMContentLoaded', () => {
  switchTab('line');
});
