import { LineItemDto } from './LineItemDto';

export interface StripeRequestDto {
  orderItems: LineItemDto[];
  orderId: number;
  street: string;
  postalCode: string;
  city: string;
}
