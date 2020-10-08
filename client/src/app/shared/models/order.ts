import { IAddress } from './address';

export interface IOrderToCreate {
    basketId: string;
    // deliveryMethodId: number;
    // shipToAddress: IAddress;
    customerid: number;
    officialid: number;
}

export interface IOrder {
    id: number;
    buyerEmail: string;
    orderDate: string;
    shipToAddress: IAddress;
    // deliveryMethod: string;
    othercosts: number;
    orderItems: IOrderItem[];
    subtotal: number;
    total: number;
    status: string;
}

export interface IOrderItem {
    categoryId: number;
    categoryName: string;
    salaryRangeMin: number;
    salaryRangeMax: number;
    quantity: number;
}
