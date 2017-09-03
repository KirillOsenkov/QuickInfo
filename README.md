# http://quickinfo.io
Extensible set of little one-off features that Google or Bing are missing, such as color and unit conversions, etc.

**[http://quickinfo.io](http://quickinfo.io)**

Isn't it frustrating when you need to calculate something simple and your search engine just gives you a list of websites full of ads instead of instantly returning the answer as you type? They're getting better at it by adding calculator, unit-conversions, currency exchange rates, stock tickers etc. but sometimes they just fail, for instance as of October 2015 search engines don't return satisfactory results for RGB(23,175,195).

I'm going to build a simple extensible website where I will accumulate simple instant solutions to one-off classes of problems where a search engine has failed me. [http://wolframalpha.com](http://wolframalpha.com) gives great results in most cases however it's too slow to be useful as a super quick one-off tool. However I like the spirit of Wolfram Alpha, but it is still not extensible by me (I can't plug my own one-off solutions). [http://duckduckgo.com](http://duckduckgo.com) is another inspiration, but the problem there is no easy extensibility in C#, although their answers collection is impressive.

All solutions are instant as-you-type results, no need for the search button.

You can preview the live site now at [http://quickinfo.io](quickinfo.io).

Supported answers for now:
 1. Color conversions - convert an RGB triplet to Hex color or hex color to RGB ("23 145 175", "#eeaaf0")
 2. Weight conversions - convert pounds to kg and vice versa ("190 pounds", "200lb", "43 kg")
 3. Temperature conversions - fahrenheit/celsius ("75f", "75 f", "75 fahrenheit", "23c", "23 c")

Feel free to fork away and host your own modified copies. I will consider accepting very high quality pull requests that contribute new solutions or fix bugs.

