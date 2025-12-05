import DisplayProfileHeader from './DisplayProfileHeader';
import WritePost from './WritePost';

function isThisAProfilePage( currentPath : string ) : boolean {
  return currentPath.startsWith('/profile/');
}

export default function HandlePageHeaders({ currentPath }: { currentPath: string }) {
  return <>
    <header className="page-header">
      {isThisAProfilePage(currentPath) && (
        <DisplayProfileHeader />
      )}
        <WritePost />
    </header>
  </>
}