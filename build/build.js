const fs = require("fs");
const {copySync} = require("fs-extra");
const { exec } = require('child_process');

// import {execFile as exec} from "child_process";
// import {spawn} from "child_process";


// exec(`${config.serverPath}/altv-server.exe`);
// exec(`cmd.exe`);
// get absolute path
// import path from "path";
// import {copySync} from "fs-extra/esm";
// import {compile} from "sass";
const {compile} = require("sass");
const child_process = require("child_process");

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
    const options = {
        outputStyle: "compressed",
        sourceMap: true,
        quietDeps: true,
    }

    // compile client
    // await compile(`./../TerraTex_RolePlay_AltV_Client\\bin\\${pathPart}\\net6.0/html/general/Styles/bootstrap/bootstrap.scss`, options);
    //await compile(`./../TerraTex_RolePlay_AltV_Client\\bin\\${pathPart}\\net6.0/html/custom/styles.scss`);

    child_process.execSync('cd frontend && npm install',{
        stdio:[0,1,2]
    });
    child_process.execSync('cd frontend && npm run build',{
        stdio:[0,1,2]
    });


    console.log("server_path: " + config.serverPath);
    // check if pid file exists and @todo: process is running
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
    // await fs.promises.rename(`./../TerraTex_RolePlay_AltV\\bin\\${pathPart}\\net6.0`, `${newFilePath}/server`);
    // await fs.promises.rename(`./../TerraTex_RolePlay_AltV_Client\\bin\\${pathPart}\\net6.0`, `${newFilePath}/client`);

    copySync(`./../TerraTex_RolePlay_AltV\\bin\\${pathPart}\\net6.0`, `${newFilePath}/server`);
    copySync(`./../TerraTex_RolePlay_AltV_Client\\bin\\${pathPart}\\net6.0`, `${newFilePath}/client`);

    copySync(`./frontend/dist`, `${newFilePath}/client/html`);

    if (wasStarted) {
        await fs.promises.writeFile(`${config.serverPath}/start.command`, "start");
    }

    console.log("build finished");
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
