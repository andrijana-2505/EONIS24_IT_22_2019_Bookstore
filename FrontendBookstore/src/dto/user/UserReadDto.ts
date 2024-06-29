import { OrderUpdateDto } from "../order/OrderUpdateDto";
import { ReviewUpdateDto } from "../review/ReviewUpdateDto";

export interface UserReadDto {
  usersId: number;
  userRole?: string;
  username: string;
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
  genre?: string;
  orders?: OrderUpdateDto[];
  reviews?: ReviewUpdateDto[];
}
