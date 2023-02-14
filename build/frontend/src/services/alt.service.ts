var onStorage: [string, { (...args: any[]): void}[]][] = [];

export class AltV {

    /**
     * Adds a subscription to an Alt:V-Event emitted by client.
     * @param EventName EventName you want to subscribe to.
     * @param AFunction The handling function you want to be executed.
     */
    public static on(EventName: string, AFunction: { (...args: any[]): void}) {
        //@ts-ignore
        alt.on(EventName, AFunction); //eslint-disable-line no-undef

        let existingEntry = onStorage.find((i) => i[0] === EventName);
        if (existingEntry) {
            existingEntry[1].push(AFunction);
        } else {
            onStorage.push([EventName, [AFunction]]);
        }
    }

    public static off(EventName:string, AFunction: { (...args: any[]): void}){
        //@ts-ignore
        alt.off(EventName, AFunction) //eslint-disable-line no-undef
    }

    /**
     * Emits an event which has to be handeled by the client.
     * @param EventName EventName you want to send.
     * @param Parameters List of paramters to be submitted to the client.
     */
    public static emit(EventName: string, ...Parameters: any[]) {
        // @ts-ignore
        alt.emit(EventName, ...Parameters);
    }
}

if (!window.hasOwnProperty('alt')) {
    console.log('Alt:V not found - register Debug Modul.');

    // @ts-ignore
    window.alt = {
        on: (EventName: string, AFunction: { (...args: any[]): void}) => {
            console.log(`Alt.on>> Name ${EventName}, ${AFunction.toString()}`);
        },
        off: (EventName: string, AFunction: { (...args: any[]): void}) => {
            console.log(`Alt.off>> Name ${EventName}, ${AFunction.toString()}`);
        },
        emit: (EventName: string, ...Parameters: any[]) => {
            console.log(`Alt.emit>> Name ${EventName}, ${JSON.stringify(Parameters)}`);
        },
        simulate: (EventName: string, ...Parameters: any[]) => {
            const storageEntry = onStorage.find((entry) => entry[0] === EventName);
            if (!storageEntry) {
                console.log("Event without subscription:", EventName);
                return;
            }
            for (let index = 0; index < storageEntry[1].length; index++) {
                let functionToCall = storageEntry[1][index];
                functionToCall.apply(null, Parameters);
            }
        }
    }
}