import React from 'react';
import { Text, View, TouchableOpacity,Image} from 'react-native';
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'

export default function Inicio({navigation}){
    const { theme } = useTheme();
    const styles = getStyles(theme);

    return(
        <View style={styles.container}>
            <Image 
                style={styles.logo}
                source={theme.logo} >
            </Image>
    

            <View style={styles.areaTitulo}>
              <Text style={styles.titulo}>Bem vindo ao WORKFLOW </Text>
              <Text style={styles.subtitulo}> Otimize seu Trabalho Conosco! </Text>
            </View>

            <TouchableOpacity
                style={styles.botao}
                onPress={()=> navigation.navigate('Login')}
            >
                <Text style={styles.textoBotao}> Login </Text>
            </TouchableOpacity>
        </View>
    )
}