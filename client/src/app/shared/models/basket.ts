import { v4 as uuidv4 } from 'uuid';

export interface IBasket {
  id: string;
  customerId: number;
  clientSecret?: string;
  paymentIntentId?: string;
  // deliveryMethodId?: number;
  othercosts?: number;
  items: IBasketItem[];
  }

export interface IBasketItem {
    id: number;
    categoryName: string;
    catName: string;
    quantity: number;
    // categoryId: number;
    industryType: string;
    skillLevel: string;

    salaryRangeMin: number;
    salaryRangeMax: number;

    /*
    ecnr: boolean;
    contractPeriodInMonths: number;
    salaryCurrency: string;
    salaryRangeMin: number;
    salaryRangeMax: number;
    salaryNegotiable: boolean;
    expDesiredInYrsMin: number;
    expDesiredInYrsMax: number;
    jobDescInBrief: string;
    jobDescAttachment: string;
    food: number;
    foodAllowance: number;
    housing: number;
    housingAllowance: number;
    transport: number;
    transportAllowance: number;
    otherAllowance: number;
    completeBy: string;
    leaveAfterMonths: number;
    leavePerYear: number;
    */
  }

export class Basket implements IBasket  {
      id = uuidv4();
      customerId: number;
     // officialId: number?;
      items: IBasketItem[] = [];
    //  clientSecret: string?;
  }

export interface IBasketTotals {
    subtotal: number;
    othercosts: number;
    total: number;
}
