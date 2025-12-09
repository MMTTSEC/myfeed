import { HubConnection, HubConnectionBuilder, HubConnectionState, LogLevel } from '@microsoft/signalr';

const HUB_URL = '/hubs/chat';

export type MessageHandler = (message: unknown) => void;

export function createChatConnection(): HubConnection {
  const token = localStorage.getItem('token');

  const connection = new HubConnectionBuilder()
    .withUrl(HUB_URL, {
      accessTokenFactory: () => token ?? '',
    })
    .withAutomaticReconnect()
    .configureLogging(LogLevel.Information)
    .build();

  return connection;
}

export async function startConnection(connection: HubConnection) {
  if (connection.state === HubConnectionState.Connected) return;
  if (connection.state === HubConnectionState.Connecting) return;
  await connection.start();
}

export async function stopConnection(connection: HubConnection) {
  if (connection.state === HubConnectionState.Disconnected) return;
  await connection.stop();
}

