import { useEffect, useState, useRef, useCallback } from 'react';
import { HubConnection } from '@microsoft/signalr';
import { createChatConnection, startConnection, stopConnection } from '../utils/signalrClient';
import { getCurrentUserId } from '../utils/api';
import type { MessageResponse } from '../utils/messagesApi';

export function useChatNotifications(activeConversationUserId?: number) {
  const [notifications, setNotifications] = useState<MessageResponse[]>([]);
  const connectionRef = useRef<HubConnection | null>(null);
  const currentUserId = getCurrentUserId();

  useEffect(() => {
    if (!currentUserId) return;

    let isMounted = true;
    let notificationHandler: ((payload: MessageResponse) => void) | null = null;

    async function setupConnection() {
      if (!connectionRef.current) {
        connectionRef.current = createChatConnection();
      }

      const connection = connectionRef.current;

      // Add notification handler (multiple handlers can exist)
      notificationHandler = (payload: MessageResponse) => {
        if (!isMounted) return;

        // Only show notification if:
        // 1. Message is for current user (they are the receiver)
        // 2. Message is NOT from the currently active conversation
        const isForCurrentUser = payload.receiverId === currentUserId;
        const isFromActiveConversation = 
          activeConversationUserId !== undefined &&
          (payload.senderId === activeConversationUserId || payload.receiverId === activeConversationUserId);

        if (isForCurrentUser && !isFromActiveConversation) {
          // Add notification
          setNotifications((prev) => {
            // Avoid duplicates
            if (prev.some(n => n.id === payload.id)) {
              return prev;
            }
            return [...prev, payload];
          });
        }
      };

      connection.on('messageReceived', notificationHandler);

      try {
        await startConnection(connection);
      } catch (err) {
        console.error('Failed to start SignalR connection for notifications', err);
      }
    }

    setupConnection();

    return () => {
      isMounted = false;
      if (connectionRef.current && notificationHandler) {
        connectionRef.current.off('messageReceived', notificationHandler);
      }
    };
  }, [currentUserId, activeConversationUserId]);

  const removeNotification = useCallback((messageId: number) => {
    setNotifications((prev) => prev.filter(n => n.id !== messageId));
  }, []);

  const clearAllNotifications = useCallback(() => {
    setNotifications([]);
  }, []);

  // Cleanup on unmount
  useEffect(() => {
    return () => {
      if (connectionRef.current) {
        stopConnection(connectionRef.current);
        connectionRef.current = null;
      }
    };
  }, []);

  return {
    notifications,
    removeNotification,
    clearAllNotifications,
    connection: connectionRef.current
  };
}

