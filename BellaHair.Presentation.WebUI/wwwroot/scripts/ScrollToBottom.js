function attachDotNetScrollHandler(element, dotnetHelper) {
	element.addEventListener('scroll', function () {
		if (element.scrollTop + element.clientHeight >= element.scrollHeight - 10) {
			dotnetHelper.invokeMethodAsync('OnScrollToBottom');
		}
	});
};
