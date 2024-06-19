namespace ZenFulcrum.EmbeddedBrowser {

/**
 * Handler for creating new windows. (See Browser.NewWindowAction.NewBrowser)
 * 
 * First, set browser.newWindowAction = NewWindowAction.NewBrowser
 * then, set browser.NewWindowHandler = myHandler
 * 
 * When the browser needs to open a new window, myHandler.CreateBrowser will be called. Create a 
 * new browser how and where you will, then return it. The new Browser will be filled with
 * the new page.
 */
public interface INewWindowHandler {

	/**
	 * Creates a new Browser object to hold a new page.
	 * The returned Browser object will then be linked and load the new page.
	 */
	Browser CreateBrowser(Browser parent);
}

}
