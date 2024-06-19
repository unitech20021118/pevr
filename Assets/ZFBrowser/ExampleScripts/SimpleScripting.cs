using System.Collections;
using UnityEngine;
using ZenFulcrum.EmbeddedBrowser;

/**
 * A simple set of examples of scripting/JavaScript interacting with a Browser.
 */
[RequireComponent(typeof(Browser))]
public class SimpleScripting : MonoBehaviour {
	private Browser browser;

	public void Start() {
		browser = GetComponent<Browser>();

		//Load some HTML. Normally you'd want to use BrowserAssets + localGame://, 
		//but let's just put everything here for simplicity/visibility right now.
		browser.LoadHTML(@"
<button style='background: green; color: white' onclick='greenButtonClicked(event.x, event.y)'>Green Button</button>


<br><br>


Username: <input type='text' id='username' value='CouchPotato47'>


<br><br>


<div id='box' style='width: 200px; height: 200px;border: 1px solid black'>
	Click ""Change Color""
</div>

<script>
function changeColor(r, g, b, text) {
	var el = document.getElementById('box');
	el.style.background = 'rgba(' + (r * 255) + ', ' + (g * 255) + ', ' + (b * 255) + ', 1)';
	el.textContent = text;
}
</script>
");

		//Set up a function. Notice how the <button> above calls this function when it's clicked.
		browser.RegisterFunction("greenButtonClicked", args => {
			//Args is an array of arguments passed to the function.
			//args[n] is a JSONNode. When you use it, it will implicitly cast to the type at hand.
			int xPos = args[0];
			int yPos = args[1];

			//Note that if, say, args[0] was a string instead of an integer we'd get default(int) above.
			//See JSONNode.cs for more information.

			Debug.Log("The <color=green>green</color> button was clicked at " + xPos + ", " + yPos);
		});
	}


	
	/** Fetches the username and logs it to the Unity console. */
	public void GetUsername() {
		browser.EvalJS("document.getElementById('username').value").Then(username => {
			Debug.Log("The username is: " + username);
		});

		//Note that the fetch above is asynchronous, this line of code will happen before the Debug.Log above:
		Debug.Log("Fetching username");
	}


	private int colorIdx;
	private Color[] colors = {
		new Color(1, 0, 0), 
		new Color(1, 1, 0), 
		new Color(1, 1, 1), 
		new Color(1, 1, 0), 
	};

	/** Changes the color of the box on the page by calling a function in the page. */
	public void ChangeColor() {
		var color = colors[colorIdx++ % colors.Length];
		browser.CallFunction("changeColor", color.r, color.g, color.b, "Selection Number " + colorIdx);
	}



	/** Fetches the username and logs it to the Unity console (alternate method). */
	public void GetUsername2() {
		StartCoroutine(_GetUsername2());
	}

	private IEnumerator _GetUsername2() {
		//This method is more useful if you are already in a coroutine, or you have a lot of logic 
		//you need to work with that is ugly/painful to express with .Then() chaining.

		var promise = browser.EvalJS("document.getElementById('username').value");

		Debug.Log("Fetching username");

		//Waits for the JS to run and get the result back to us.
		yield return promise.ToWaitFor();

		Debug.Log("The username is: " + promise.Value);
	}
}
