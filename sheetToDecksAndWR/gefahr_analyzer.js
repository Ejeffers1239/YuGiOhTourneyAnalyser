'use strict';
let yrp = require('yrp');
const {google} = require('googleapis');
let fs = require('fs');
const {file} = require("googleapis/build/src/apis/file");
const {parse} = require("csv-parse")
let apiKey = 'AIzaSyBDfV6miuJyT-rMYA1U8_LNThS4U0flqg8'; //REPLACE WITH YOUR OWN
let winrates = '';

function sleep(ms) {
	return new Promise((resolve) => {
		setTimeout(resolve, ms);
	});
}

const drive = google.drive({
	version: 'v3',
	auth: apiKey,
	key: apiKey
});

async function main() {
	fs.createReadStream("sheet.csv").pipe(parse({delimiter: ","}))
		.on("data", async function (row) {
			let wins = parseInt(row[4]);
			let losses = parseInt(row [5]);
			let games = wins + losses;
			let wr = wins/(wins + losses)
			let lr = 1-wr;
			let winURL = '';
			let lossURL = '';
			winURL = row[13];
			lossURL = row[14];
			let winID = winURL.split("=")[1];
			let lossID = lossURL.split("=")[1]
				//await printFile(fileID);
				//winrates += deckname + "," + wr +"\n";
				await drive.files.get({fileId: winID}, async(er, re) => { // Added
					if (er) {
						console.log(er);
						return;
					}
					fs.appendFile("winrates.txt", re.data.name + "," + wr + "," + games + "\n", (er, re) => { // Added
						if (er) {
							console.log(er);
							return;
						}
					});
					await drive.files.get({fileId: lossID}, async(er, re) => { // Added
						if (er) {
							console.log(er);
							return;
						}
						fs.appendFile("winrates.txt", re.data.name + "," + lr + "," + games + "\n", (er, re) => { // Added
							if (er) {
								console.log(er);
								return;
							}
						});
				});
			});
		})
		.on("error", function (error) {
			console.log(error.message);
		})
		.on("end", async function () {
			console.log(winrates);
			/*await fs.writeFile("winrates.txt", winrates,(er, re) => { // Added
				if (er) {
					console.log(er);
					return;
				}
			});

			 */

		});
}
main();