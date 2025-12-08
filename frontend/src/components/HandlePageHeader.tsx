import DisplayProfileHeader from './DisplayProfileHeader';
import WritePost from './WritePost';

function isThisAProfilePage( currentPath : string ) : boolean {
  return currentPath.startsWith('/profile/');
}

interface HandlePageHeadersProps {
  currentPath: string;
  onPostCreated?: () => void;
}

export default function HandlePageHeaders({ currentPath, onPostCreated }: HandlePageHeadersProps) {
  return <>
    <header className="page-header">
      {isThisAProfilePage(currentPath) && (
        <DisplayProfileHeader />
      )}
        <WritePost onPostCreated={onPostCreated} />
    </header>
  </>
}