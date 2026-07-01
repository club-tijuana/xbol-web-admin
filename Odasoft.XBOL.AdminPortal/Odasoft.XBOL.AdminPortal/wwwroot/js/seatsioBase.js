/** @typedef {import('seatsio').EventManager} SeatsioChart */

let loadingPromise = null;

/** Loads the seats.io chart.js SDK from CDN if not already present. */
export async function ensureSeatsioLoaded() {
  if (typeof seatsio !== "undefined") return;
  if (!loadingPromise) {
    loadingPromise = new Promise((resolve, reject) => {
      const script = document.createElement("script");
      script.src = "https://cdn-na.seatsio.net/chart.js";
      script.onload = resolve;
      script.onerror = reject;
      document.head.appendChild(script);
    });
  }
  await loadingPromise;
}

/**
 * Destroys the chart instance and removes its iframe.
 * @param {SeatsioChart} chart
 */
export function destroyChart(chart) {
  chart?.destroy();
}

/**
 * Zooms to a seat and pulses it to draw attention.
 * @param {SeatsioChart} chart
 * @param {string} seatId
 */
export function focusSeat(chart, seatId) {
  chart?.zoomToObjects([seatId]);
  chart?.pulse([seatId]);
}

/**
 * Stops pulsing a previously focused seat.
 * @param {SeatsioChart} chart
 * @param {string} seatId
 */
export function unfocusSeat(chart, seatId) {
  chart?.unpulse([seatId]);
}

/**
 * Unpulses and deselects a single seat.
 * @param {SeatsioChart} chart
 * @param {string} seatId
 */
export function deselectSeat(chart, seatId) {
  chart?.unpulse([seatId]);
  chart?.deselectObjects([seatId]);
}

/**
 * Deselects all currently selected objects.
 * @param {SeatsioChart} chart
 */
export function clearSelection(chart) {
  chart?.clearSelection();
}

/**
 * Updates the chart configuration at runtime (e.g. filtered categories).
 * @param {SeatsioChart} chart
 * @param {Object} config
 */
export function changeConfig(chart, config) {
  chart?.changeConfig(config);
}

/**
 * Zooms the chart to the currently filtered categories.
 * @param {SeatsioChart} chart
 */
export function focusFilteredCategories(chart) {
  chart?.zoomToFilteredCategories();
}

/**
 * Renders a read-only chart for a specific venue map.
 * @param {string} containerId - The ID of the HTML element to render into.
 * @param {string} workspaceKey - Your seats.io public workspace key.
 * @param {string} chartKey - The key of the map/chart to load.
 * @returns {Promise<SeatsioChart>} The rendered chart instance.
 */
export async function renderVenueMapAsync(containerId, workspaceKey, chartKey) {
  await ensureSeatsioLoaded();

  const chart = new seatsio.SeatingChart({
    divId: containerId,
    workspaceKey: workspaceKey,
    chartKey: chartKey,
    mode: 'read-only' // Evita que los asientos sean seleccionables/comprables
  }).render();

  return chart;
}
