export interface BookUpdateDto {
  bookId: number;
  bookTitle: string;
  bookAuthor: string;
  publishingYear: Date;
  publisher: string;
  bookPrice: number;
  available: number;
  categoryId?: number;
}
