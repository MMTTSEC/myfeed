import DisplayProfileHeader from './DisplayProfileHeader';
import WritePost from './WritePost';
import { getCurrentUserId } from '../utils/api';

function isThisAProfilePage( currentPath : string ) : boolean {
  return currentPath.startsWith('/profile/');
}

interface HandlePageHeadersProps {
  currentPath: string;
  onPostCreated?: () => void;
  profileUserId?: number;
}

export default function HandlePageHeaders({ currentPath, onPostCreated, profileUserId }: HandlePageHeadersProps) {
  const currentUserId = getCurrentUserId();
  const isOwnProfile = !profileUserId || profileUserId === currentUserId;
  
  return <>
    <header className="page-header">
      {isThisAProfilePage(currentPath) && (
        <DisplayProfileHeader profileUserId={profileUserId} />
      )}
      {isOwnProfile && (
        <WritePost onPostCreated={onPostCreated} />
      )}
    </header>
  </>
}