# http://quickinfo.io
Extensible set of little one-off features that Google or Bing are missing, such as color and unit conversions, etc.

[!screenshot](https://github.com/KirillOsenkov/QuickInfo/raw/master/docs/ColorChart.png)

## Motivation

There is a class of search engine queries that search engines aren't great at without dedicated support. These queries have exactly one well-defined answer that is computable or readily available. Search engines by default just show a list of (often poor quality and full of ads) websites instead of showing the answer directly as you type.

Search engines are getting better at it by implementing dedicated "answer" UI, without the need to visit any websites. Examples are calculator, unit conversions, currency exchange rates, stock tickers, etc. But sometimes they still fail, for instance as of October 2015 search engines don't return satisfactory results for RGB(23,175,195). If you have to visit a site for an answer like this, the search engines have failed here.

Quickinfo.io attempts to fill the gap when the search engine doesn't provide an "answer", or their answer is not satisfactory. One should note that http://duckduckgo.com is known for its great extensive collection of answers. http://wolframalpha.com is another example of an "answer" website, which gives great answers, but it also doesn't cover many scenarios, and is also too slow to be useful as a quick one-off lookup tool (doesn't provide results in real-time as you type). I like the vision of Wolfram Alpha, but it is still not extensible by me (I can't plug my own one-off solutions). I want a platform where I can easily add new answers myself using a very simple C# extensibility API. [http://duckduckgo.com](http://duckduckgo.com)'s answers collection is impressive, but there is no easy extensibility in C#.

All the answers on http://quickinfo.io are instant as-you-type results, no need for the search button.

## Currently supported answers

1. Unit conversions:
   1. Weight: pounds to kg and vice versa (`190 pounds`, `200lb`, `43 kg in lb`)
   1. Temperature: fahrenheit/celsius (`75f`, `75 f`, `75 fahrenheit`, `23c`, `23 c`, `23 c to f`)
   1. Distance: `12 miles`, `100 miles in km`
   1. Speed
   1. Volume
   1. Area: `1670 sq.ft`
   1. Fuel efficiency (`29mpg`)
1. Hex/decimal (just type a number)
1. Ascii table `ascii`
1. List of colors: `color`, `colors`
1. Color conversions - convert an RGB triplet to Hex color or hex color to RGB (`23 145 175`, `#eeaaf0`)
1. Factor an integer `2520`
1. Generate a random Guid `guid`
1. My IP address `ip`
1. Math/arithmetic expressions `(24 * 365) * 4 - 1`, `22/7 - pi`, `rnd`, `Random(100)`
1. Quick sum/average/product of a list of numbers, sort a list (separated by space or comma) `3 7 21 3 3 2 11`
1. Unicode: type a codepoint to see what char it is: `U+2021` `\u3333`
1. Url decode: `2%20%2B%203` shows the original string (fun exercise: `2%2525252520%252525252B%25252525205`)

Type `?` for help.

## Historical information

It all started when I caught myself using the Windows Calc to convert a hex color to RGB. I thought to myself: _this is ridiculous_, I'm sitting in front of a supercomputer and punching buttons one by one instead of getting an immediate answer as I type it in.

I kept catching myself, _how do I generate a random Guid from command line_? _Which is a good unit-converter app without ads_? _What is 130000 * (96 - 36) / 192_? I've realized that there are apps and websites to answer all these, but it will take me longer to find a good app than to calculate the answer myself.

So I got into the mindset of "does this question have a single, well defined, computable answer"? "Can I type the question into a simple search box and expect a computer to give an immediate result?" Turns out, this class of questions is more expansive than one might think. I'm thinking of adding ToString() syntax for DateTime, string.Format cheat sheet, some man pages that I often look up, basically make carefully curated information easily accessible from a simple search box. I think it'll be beneficial to centralize all these little lookup utilities and make it so I can customize and extend it to fit my needs.

## Contributing

Feel free to open issues to suggest/propose ideas. If you want to contribute a PR, please open an issue first so we can discuss. I will consider accepting high quality pull requests, however in each case I will decide whether the PR adds a solution that fits the spirit of what I want to achieve. Sorry if I decline too specific or exotic functionality. I also can't add answers that won't be useful to the majority of users.

Longer term I want to think about letting people easily add their own answers (and store them in their own private account, like OneNote), but this is far out in the future and I'm not sure I have the time and resources to maintain that.

