//Dennis
//JS script som kan (via en ref) kobles til en div eller anden scroll view, og vil køre metoden
//	OnScrollToBottom hver gang der scrolles til bunden af viewet

function attachDotNetScrollHandler(element, dotnetHelper) {
	element.addEventListener('scroll', function () {
		if (element.scrollTop + element.clientHeight >= element.scrollHeight - 10) {
			dotnetHelper.invokeMethodAsync('OnScrollToBottom');
		}
	});
};
