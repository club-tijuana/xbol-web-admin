let loadingPromise = null;

async function ensureSeatsioLoaded() {
  if (typeof seatsio !== 'undefined') {
    return;
  }

  if (!loadingPromise) {
    loadingPromise = new Promise((resolve, reject) => {
      const script = document.createElement('script');
      script.src = 'https://cdn-na.seatsio.net/chart.js';
      script.onload = resolve;
      script.onerror = reject;
      document.head.appendChild(script);
    });
  }

  await loadingPromise;
}

export async function renderChart(containerId, config, dotNetHelper) {
  await ensureSeatsioLoaded();

  const chart = new seatsio.SeatingChart({
    divId: containerId,
    workspaceKey: config.workspaceKey,
    event: config.event,
    session: config.session,
    pricing: config.pricing,
    channels: config.channels,
    priceFormatter: price => '$' + price,
    onObjectSelected: obj => {
      dotNetHelper.invokeMethodAsync('HandleSeatSelected', obj.id, obj.pricing?.price ?? 0, obj.category?.label);
    },
    onObjectDeselected: obj => {
      dotNetHelper.invokeMethodAsync('HandleSeatDeselected', obj.id);
    }
  });

  await chart.render();
  return chart;
}

export function getHoldToken(chart) {
  return chart?.holdToken ?? null;
}

export function destroyChart(chart) {
  chart?.destroy();
}
