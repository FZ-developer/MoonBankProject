import { Box, Button, ItemActivity, ItemContainer, Text } from '../../shared/styles';

interface Props {
  icon: string;
  serviceTitle: string;
  serviceDescription: string;
  date?: string;
  quantity: number;
  typeItem: 'reward' | 'service' | 'activity';
  totalPoints?: number;
  idService?: number;
  transaction?: () => void;
}

export const Activity = ({
  date,
  icon,
  quantity,
  serviceDescription,
  serviceTitle,
  transaction,
  typeItem,
  totalPoints,
}: Props) => {
  const isEnoughPoints = typeItem === 'reward' && (totalPoints as number) >= quantity;

  return (
    <ItemContainer>
      <ItemActivity>
        <Box>
          <img src={icon} alt={icon} />
          <Box direction='column' marginLeft='10px'>
            <Text fontWeight='Bold_700' fontSize='15px' fontSizeVar={{ large: '13px', medium: '11px', min: '9px' }}>
              {serviceTitle}
            </Text>
            <Text fontWeight='Normal_400' fontSize='13px' fontSizeVar={{ large: '11px', medium: '9px', min: '7px' }}>
              {serviceDescription}
            </Text>
          </Box>
        </Box>
        <Box direction='column'>
          <Text fontSize='14px' fontWeight='DemiBold_600' fontSizeVar={{ large: '12px', medium: '11px', min: '9px' }}>
            {date}
          </Text>
          <Text
            fontSize='12px'
            style={{ color: isEnoughPoints ? 'var(--LightGreen)' : 'var(--LightRed)' }}
            fontSizeVar={{ large: '10px', medium: '9px', min: '8px' }}>
            {typeItem === 'service' ? `$ ${quantity} ` : typeItem === 'reward' ? `${quantity} px` : `- ${quantity} `}
          </Text>
        </Box>
      </ItemActivity>
      {typeItem !== 'activity' && (
        <Button width='100%' variant='blue' onClick={transaction}>
          Perform transaction
        </Button>
      )}
    </ItemContainer>
  );
};
