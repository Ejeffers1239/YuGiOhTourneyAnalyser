
cd sheetToDecksAndWR
md Decks
cd Decks
del *.ydk
cd ..
del sheet.csv
del winrates.txt
cd ..
cd TourneyAnalyser
md Decks
cd Decks
del *.ydk
cd ..
del winrates.txt
cd ..
copy Decks sheetToDecksAndWR\Decks
copy Decks TourneyAnalyser\Decks
copy sheet.csv sheetToDecksAndWR
cd sheetToDecksAndWR
node gefahr_analyzer.js
move winrates.txt ..\TourneyAnalyser
cd ..
cd TourneyAnalyser
dotnet run Program.cs
move analysis.txt ..
cd ..
