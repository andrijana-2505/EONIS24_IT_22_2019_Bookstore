export interface ReviewUpdateDto {
  reviewId: number;
  rating: number;
  reviewDate?: Date;
  usersId?: number;
  bookId?: number;
}