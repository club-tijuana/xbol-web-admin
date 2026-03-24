/**
 * EventManagerChart JS interop module.
 * Renders a seats.io EventManager for admin operations
 * (e.g. manageForSaleConfig, manageObjectStatuses).
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
} from "/js/seatsioBase.js";

export {
  destroyChart,
  focusSeat,
  unfocusSeat,
  deselectSeat,
  clearSelection,
  changeConfig,
  focusFilteredCategories,
};

/**
 * Renders an EventManager chart for admin/availability operations.
 * @param {string} containerId - DOM element ID for the chart container
 * @param {Object} config - Chart configuration from Blazor
 * @param {DotNetObjectReference} dotNetHelper - Blazor interop reference
 * @returns {Promise<SeatsioChart>}
 */
export async function renderChart(containerId, config, dotNetHelper) {
  await ensureSeatsioLoaded();

  const chart = new seatsio.EventManager({
    divId: containerId,
    secretKey: config.secretKey,
    event: config.event,
    language: config.language,
    mode: config.mode,
    onObjectSelected: (obj) => {
      dotNetHelper.invokeMethodAsync(
        "HandleSeatSelected",
        obj.id,
        obj.pricing?.price ?? 0,
        obj.category?.label,
      );
    },
    onObjectDeselected: (obj) => {
      dotNetHelper.invokeMethodAsync("HandleSeatDeselected", obj.id);
    },
    onObjectStatusChanged: (obj) => {
      dotNetHelper.invokeMethodAsync(
        "LogActionToApiHookAsync",
        "ObjectStatusChanged",
        {
          objectId: obj.id,
          status: obj.status,
          extraData: obj.extraData,
        },
      );
    },
  });

  await chart.render();
  return chart;
}
