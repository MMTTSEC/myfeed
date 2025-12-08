import 'bootstrap-icons/font/bootstrap-icons.css';

function isProfileOrMessagesPage(currentPath: string): boolean {
  const pathname = currentPath.split(/[?#]/)[0];
  return /^\/(messages|profile)(\/|$)/.test(pathname);
}

function toggleLeftColumnActive() {
  const left = document.querySelector('section.left-column');
  const right = document.querySelector('section.right-column');
  if (!left) return;

  const leftIsActive = left.classList.contains('active');
  if (leftIsActive) {
    // turn left off
    left.classList.remove('active');
  } else {
    // turn left on and ensure right is off
    left.classList.add('active');
    if (right) right.classList.remove('active');
  }
}

function toggleRightColumnActive() {
  const left = document.querySelector('section.left-column');
  const right = document.querySelector('section.right-column');
  if (!right) return;

  const rightIsActive = right.classList.contains('active');
  if (rightIsActive) {
    // turn right off
    right.classList.remove('active');
  } else {
    // turn right on and ensure left is off
    right.classList.add('active');
    if (left) left.classList.remove('active');
  }
}

export default function MobileHeader({ currentPath }: { currentPath: string }) {
  return <>
    <header className="mobile-header">
      <nav className="mobile-navigation">
        <ul>
          <li>
            <a href="#" onClick={(e) => { e.preventDefault(); toggleLeftColumnActive(); }}>
              <i className="bi bi-list toggle-left-section"></i>
            </a>
          </li>
          {isProfileOrMessagesPage(currentPath) && (
            <li>
              <a href="#" onClick={(e) => { e.preventDefault(); toggleRightColumnActive(); }}>
                <i className="bi bi-card-list toggle-right-section"></i>
              </a>
            </li>
          )}
        </ul>
      </nav>
    </header>
  </>
}