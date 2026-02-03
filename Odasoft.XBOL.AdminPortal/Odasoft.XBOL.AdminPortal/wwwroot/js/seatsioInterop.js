let activeChart = null;

window.renderSeatsioChart = (containerId, config, dotNetHelper) => {
  if (typeof seatsio === 'undefined') {
    console.error("Seats.io library not loaded.");
    return;
  }

  activeChart = new seatsio.SeatingChart({
    divId: containerId,
    workspaceKey: config.workspaceKey,
    event: config.event,
    session: config.session,
    pricing: config.pricing,
    channels: config.channels,
    selectedObjects: config.selectedObjects,
    priceFormatter: function (price) {
      return '$' + price;
    },
    onObjectSelected: function (object) {
      dotNetHelper.invokeMethodAsync('HandleSeatSelected', object.id, object.pricing?.price ?? 0, object.category?.label);
    },
    onObjectDeselected: function (object) {
      dotNetHelper.invokeMethodAsync('HandleSeatDeselected', object.id);
    },
    onChartRendered: function () {
      dotNetHelper.invokeMethodAsync('HandleChartRendered');
    }
  }).render();
};

window.getHoldToken = () => {
  if (activeChart) {
    return activeChart.holdToken;
  }
  return null;
};
