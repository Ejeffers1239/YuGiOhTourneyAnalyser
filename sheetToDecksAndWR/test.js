async function main()
{
    const {Replay} = require("yrp");
    var buf = Buffer.from('replays/2022-09-18 14-41-48.yrp');
        const replay = await Replay.fromFile('replays/2022-09-18 14-41-48.yrp');
        const decks = replay.getDecks();
        let winname = "(winner)_LastReplay - cody beeber1.ydk";
        let lossname = "(loser)_LastReplay - cody beeber1.ydk";
        let wr = 2 / 3;
        let lr = 1 - wr;
        let winydk = '#main\n';
        let lossydk = '#main\n';
        for (const numkey in decks[0].main) {
            winydk += decks[0][numkey] + '\n';
        }
        for (const numkey in decks[1].main) {
            winydk += decks[1][numkey] + '\n';
        }
        for (const numkey in decks[0].extra) {
            lossydk += decks[0][numkey] + '\n';
        }
        for (const numkey in decks[1].extra) {
            lossydk += decks[1][numkey] + '\n';
        }
}


main();