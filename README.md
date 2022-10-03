YugiohTourney Analyzer Takes a list of decks and a properly formatted csv
and processes it to a list of card winrates

HOW TO USE:

-1.
npm i yrp
npm i googleapis
npm i csv-parse

0.
Go to sheetToDecksAndWR/gefahr_analyzer.js, and replace
the API key on line 7 with your own from:
https://console.cloud.google.com/apis/credentials?_ga=2.178904548.1151550492.1664311305-1971446735.1653946488

or otherwise have your IP added to MY key.

1.
Place all ydk files in the "Decks" folder, anything other than 
ydk is ignored

2.
Export the properly formatted sheet and place in THIS FOLDER 
and name it "sheet.csv

3. 
Run "run.bat" the program may ask for some winrate input when it
fails to scrape it automatically

4.
Barring highly likely issues, analysis.txt will be in THIS FOLDER.
