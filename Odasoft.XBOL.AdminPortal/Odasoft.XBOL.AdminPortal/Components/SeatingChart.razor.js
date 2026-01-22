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
    // TODO: Add this config to component parameters
    multiSelectEnabled: true,
    categoryFilter: {
      enabled: true,
      multiSelect: true,
      zoomOnSelect: true
    },
    language: 'es',
    priceFormatter: price => '$' + price,
    onObjectSelected: obj => {
      dotNetHelper.invokeMethodAsync('HandleSeatSelected', obj.id, obj.pricing?.price ?? 0, obj.category?.label);
    },
    onObjectDeselected: obj => {
      dotNetHelper.invokeMethodAsync('HandleSeatDeselected', obj.id);
    },
    onHoldSucceeded: (objects, ticketTypes) => {
      dotNetHelper.invokeMethodAsync('HandleHoldSucceeded', objects.map(o => o.id), ticketTypes);
    },
    onHoldFailed: (objects, ticketTypes) => {
      dotNetHelper.invokeMethodAsync('HandleHoldFailed', objects.map(o => o.id), ticketTypes);
    },
    onHoldTokenExpired: () => {
      dotNetHelper.invokeMethodAsync('HandleSessionExpired');
    },
    onSessionInitialized: holdToken => {
      dotNetHelper.invokeMethodAsync('HandleSessionInitialized', holdToken.token, holdToken.expiresInSeconds);
    },
    onHoldCallsInProgress: () => {
      dotNetHelper.invokeMethodAsync('HandleHoldCallsInProgress');
    },
    onHoldCallsComplete: () => {
      dotNetHelper.invokeMethodAsync('HandleHoldCallsComplete');
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

export function focusSeat(chart, seatId) {
  chart?.zoomToSelectedObjects([seatId]);
  chart?.pulse([seatId]);
}

export function unfocusSeat(chart, seatId) {
  chart?.unpulse([seatId]);
}

export function deselectSeat(chart, seatId) {
  chart?.unpulse([seatId]);
  chart?.deselectObjects([seatId]);
}

export function clearSession() {
  sessionStorage.removeItem('seatsio');
}
