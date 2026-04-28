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

  const chart = new seatsio.EventManager({
    divId: containerId,
    secretKey: config.secretKey,
    mode: config.mode,
    event: config.event,
    session: config.session,
    holdToken: config.holdToken,
    pricing: {
      prices: config.pricing,
      priceFormatter: price => '$' + price
    },
    channels: config.channels,
    selectedObjects: config.selectedObjects,
    language: config.language,
    categoryFilter: config.categoryFilter,
    onObjectSelected: obj => {
      let price = obj.pricing?.price ?? obj.pricing?.ticketTypes?.find((seat) => seat.ticketType == obj.selectedTicketType)?.price ?? 0;
      dotNetHelper.invokeMethodAsync('HandleSeatSelected', obj.id, price, obj.category?.label, obj.selectedTicketType);
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

export function getSelectedSeats(chart) {
  return chart?.selectedObjects ?? [];
}

export function destroyChart(chart) {
  chart?.destroy();
}

export function focusSeat(chart, seatId) {
  chart?.zoomToSelectedObjects([seatId]);
  chart?.pulse([seatId]);
}

export function focusFilteredCategories(chart) {
  chart?.zoomToFilteredCategories();
}

export function unfocusSeat(chart, seatId) {
  chart?.unpulse([seatId]);
}

export function deselectSeat(chart, seatId) {
  chart?.unpulse([seatId]);
  chart?.deselectObjects([seatId]);
}

export function deselectSeats(chart, seats) {
  chart?.unpulse(seats);
  chart?.deselectObjects(seats);
}

export function clearSelection(chart) {
  chart?.clearSelection();
}

export async function trySelectObjects(chart, seats, dotNetHelper) {
  try {
    await chart?.trySelectObjects(seats);
  } catch (e) {
    // TODO: Marcar ticket como vendido o eliminarlo de la lista
    dotNetHelper.invokeMethodAsync('NotifyError', "No se pudo seleccionar uno o mas asientos.");
  }  
}

export async function doSelectObjects(chart, seats, dotNetHelper) {
  try {
    await chart?.doSelectObjects(seats);
  } catch (e) {
    dotNetHelper.invokeMethodAsync('NotifyError', "No se pudo seleccionar uno o mas asientos."); 
  }
}

export function clearSession() {
  sessionStorage.removeItem('seatsio');
}

export function changeConfig(chart, config) {
  chart?.changeConfig(config);
}
