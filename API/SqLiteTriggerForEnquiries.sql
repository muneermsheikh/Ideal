CREATE TRIGGER GenerateNewEnquiryNo [AFTER] INSERT
ON Enquiries
BEGIN
 Update Enquiries Set EnquiryNo = Select Max(EnquiryNo) + 1 From Enquiries where Id = NEW.Id;
END;