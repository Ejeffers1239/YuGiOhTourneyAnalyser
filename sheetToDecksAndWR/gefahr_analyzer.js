'use strict';
let yrp = require('yrp');
const {google} = require('googleapis');
let fs = require('fs');
const {file} = require("googleapis/build/src/apis/file");

let winners,
	losers,
	WR,
	winName,
	lossName;
let apiKey = 'AIzaSyBDfV6miuJyT-rMYA1U8_LNThS4U0flqg8'; //security be damned, I just want this to work easily

const drive = google.drive({
	version: 'v3',
	auth: apiKey,
	key: apiKey
});

// Each API may support multiple versions. With this sample, we're getting
// v3 of the blogger API, and using an API key to authenticate.

async function printFile(fileId) {
	const res = await drive.files.get({fileId: fileId}, (er, re) => { // Added
		if (er) {
			console.log(er);
			return;
		}
		console.log(re.data.name);
		return re.data.name; // Modified

	});

}




	console.log("can I see the console at all?");
	printFile('1Scvryf6g1Cof2Ao_T58VoykL1z1aJNHW');


