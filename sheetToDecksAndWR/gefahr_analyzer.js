'use strict';
let winners,
	losers,
	WR,
	winName,
	lossName;
let apiKey = 'AIzaSyAOriFwczjdWKULoZERRJuAaRbgjmScCHo'; //security be damned, I just want this to work easily

const {google} = require('googleapis');

// Each API may support multiple versions. With this sample, we're getting
// v3 of the blogger API, and using an API key to authenticate.
const files = google.drive.file({
  version: 'v3',
  auth: 'AIzaSyAOriFwczjdWKULoZERRJuAaRbgjmScCHo'
});

let params = {
  fileId: '1MwrqybRq6WSU0JbPQmVyKkqEmaStNrt9'
};





function init(){
	// get the blog details
files.get(params, (err, res) => {
  if (err) {
    console.error(err);
    throw err;
  }
  console.log(`The file title is ${res.data.title}`);
});
}