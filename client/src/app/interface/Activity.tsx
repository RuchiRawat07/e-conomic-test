export interface ActivityInterface {
    id: number;
    activityName: string;
}

export interface AddEntryInterface {
    projectId : number;
    activityId : number;
    startTime: string;
    endTime: string;
}