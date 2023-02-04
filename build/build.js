import fs from "fs";

import {execFile as exec} from "child_process";
import {spawn} from "child_process";


// exec(`${config.serverPath}/altv-server.exe`);
// exec(`cmd.exe`);
// get absolute path
import path from "path";

var args = process.argv.slice(2).map((arg) => arg.toLowerCase());

let wasStarted = false;
let pathPart = "Release";
let config = {};
if (args.indexOf('--env=debug') > -1) {
  console.log('deploy local');
  config = JSON.parse(fs.readFileSync('./config.local.json').toString());
  pathPart = "Debug";
} else {
  console.log('deploy to server');
  config = JSON.parse(fs.readFileSync('./../config.prod.json').toString());
}

console.log(config);


async function build() {
    // check if pid file exists and process is running
    if (fs.existsSync(`${config.serverPath}/pid.txt`)) {
        await fs.promises.writeFile(`${config.serverPath}/stop.command`, "stopcmd");

        await waitUntil(() => {
            return !fs.existsSync(`${config.serverPath}/pid.txt`);
        });
        wasStarted = true;

        await sleep(2000);
        console.log("server stopped");
    }

    // // delete recursive old files
    const oldFilePath = `${config.serverPath}/resources/terratex-rp`;
    await fs.promises.rm(oldFilePath, { recursive: true, force: true });
    //
    const newFilePath = `${config.serverPath}/resources/terratex-rp`;
    // // create directory
    await fs.promises.mkdir(newFilePath);
    //
    // // copy new files
    await fs.copyFileSync(`./../resource.toml`, `${newFilePath}/resource.toml`);
    // // move directory
    await fs.promises.rename(`./../TerraTex_RolePlay_AltV\\bin\\${pathPart}\\net6.0`, `${newFilePath}/server`);
    await fs.promises.rename(`./../TerraTex_RolePlay_AltV_Client\\bin\\${pathPart}\\net6.0`, `${newFilePath}/client`);

    // if (wasStarted) {
    //     await sleep(2000);
    //     console.log("start server");
    //     const absolutePath = path.resolve(`${config.serverPath}`);
    //     console.log(absolutePath);
    //     exec(`cd ${absolutePath} && START start.bat`);
    //     spawn('cmd.exe', ['/c', `cd ${absolutePath} && START start.bat`], {
    //         detached: true,
    //         stdio: 'ignore'
    //     }).unref();
    // }
    // @todo: start server - maybe in pipeline
}

build();

async function waitUntil(checkTask, timeout = 10000, interval = 1000) {
    let time = 0;
    while (time < timeout) {
        if (await checkTask()) {
            return true;
        }
        await sleep(interval);
        time += interval;
    }
    return false;
}

async function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}
