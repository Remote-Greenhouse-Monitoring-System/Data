
GO

INSERT INTO [dbo].[Measurements]
           ([GreenhouseId],
		   [Temperature],
		   [Humidity],
		   [Co2],
		   [Light],
		[Timestamp]
           )
     SELECT 
			114,
           [Temp],
		   [RH, %],
		   [CO2, ppm],
		   10764,
		   [Date Time, GMT +0100]
	FROM
		[dbo].[001]
		 
GO


