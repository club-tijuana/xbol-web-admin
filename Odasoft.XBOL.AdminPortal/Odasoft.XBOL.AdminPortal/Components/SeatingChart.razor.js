/**
 * SeatingChart JS interop module.
 * Renders a seats.io EventManager for the customer ordering flow
 * (mode "createOrder" with hold-token session management + pricing).
 *
 * @typedef {import('seatsio').EventManager} SeatsioChart
 */
import {
  ensureSeatsioLoaded,
  destroyChart,
  focusSeat,
  unfocusSeat,
  deselectSeat,
  clearSelection,
  changeConfig,
  focusFilteredCategories,
} from "../js/seatsioBase.js";

export {
  destroyChart,
  focusSeat,
  unfocusSeat,
  deselectSeat,
  clearSelection,
  changeConfig,
  focusFilteredCategories,
};

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

export function deselectSeats(chart, seats) {
  chart?.unpulse(seats);
  chart?.deselectObjects(seats);
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
