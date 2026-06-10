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
    objectIcon: (obj, defaultIcon, extraConfig) => {
      if (obj.objectType === 'Seat' && !obj.forSale) {
        return 'lock';
      }
      return defaultIcon;
    },
    objectColor: (obj, defaultColor, extraConfig) => {
      if (!obj.forSale && obj.extraData?.color) {
        return obj.extraData.color;
      }
      return defaultColor;
    },
    tooltipInfo: (obj) => {
      if (!obj.forSale && obj.extraData?.reason) {
        const label = config.blockReasonLabel;
        return label ? `${label}: ${obj.extraData.reason}` : obj.extraData.reason;
      }
      return '';
    },
    onObjectSelected: (obj) => {
      dotNetHelper.invokeMethodAsync(
        "HandleSeatSelected",
        obj.id,
        obj.pricing?.price ?? 0,
        obj.pricing?.priceListItemId ?? 0,
        obj.category?.label,
        obj.forSale ?? true,
        obj.extraData?.reason ?? null,
        obj.extraData?.color ?? null,
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
