export default interface Post {
  id: string;
  author: string;
  authorId?: number;
  createdAt: string;
  content: string;
  likesCount: string;
}