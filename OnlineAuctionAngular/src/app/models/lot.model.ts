import { Bid } from './bid.model';
import { Category } from './category.model';

export interface Lot{
    lotId: number;
    name: string;
    initialPrice: number;
    beginDate: Date;
    endDate: Date;
    description: string;
    status: AuctionStatus;
    bestBid: Bid;
    currentPrice: number;
    imageUrl: string;
    image: string;
    userName: string;
    category: Category;
    bids: Bid[];
}

enum AuctionStatus {
    New = "New",
    Active = "Active",
    Finished = "Finished"
}