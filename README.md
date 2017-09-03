# http://quickinfo.io
Extensible set of little one-off features that Google or Bing are missing, such as color and unit conversions, etc.

**[http://quickinfo.io](http://quickinfo.io)**

There is a class of search engine queries that search engines aren't great at without dedicated support. These queries have exactly one well-defined answer that is computable or readily available. Search engines by default just show a list of (often poor quality and full of ads) websites instead of showing the answer directly as you type.

Search engines are getting better at it by implementing dedicated "answer" UI, without the need to visit any websites. Examples are calculator, unit conversions, currency exchange rates, stock tickers, etc. But sometimes they still fail, for instance as of October 2015 search engines don't return satisfactory results for RGB(23,175,195). If you have to visit a site for an answer like this, the search engines have failed here.

Quickinfo.io attempts to fill the gap when the search engine doesn't provide an "answer", or their answer is not satisfactory. One should note that http://duckduckgo.com is known for its great extensive collection of answers. http://wolframalpha.com is another example of an "answer" website, which gives great answers, but it also doesn't cover many scenarios, and is also too slow to be useful as a quick one-off lookup tool (doesn't provide results in real-time as you type). I like the vision of Wolfram Alpha, but it is still not extensible by me (I can't plug my own one-off solutions). I want a platform where I can easily add new answers myself using a very simple C# extensibility API. [http://duckduckgo.com](http://duckduckgo.com)'s answers collection is impressive, but there is no easy extensibility in C#.

All the answers on http://quickinfo.io are instant as-you-type results, no need for the search button.

Supported answers for now:
 1. Color conversions - convert an RGB triplet to Hex color or hex color to RGB ("23 145 175", "#eeaaf0")
 2. Weight conversions - convert pounds to kg and vice versa ("190 pounds", "200lb", "43 kg")
 3. Temperature conversions - fahrenheit/celsius ("75f", "75 f", "75 fahrenheit", "23c", "23 c")
 
Feel free to fork away and host your own modified copies. I will consider accepting very high quality pull requests that contribute new solutions or fix bugs. In each case I will decide whether the PR adds a solution that fits the spirit of what I want to achieve. Sorry if I decline too specific or exotic functionality.

Longer term I want to think about letting people easily add their own answers, but this is far ou in the future.

