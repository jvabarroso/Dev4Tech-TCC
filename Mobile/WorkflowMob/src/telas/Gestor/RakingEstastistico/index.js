import React, { useState } from 'react';
import { Text, View, Image, FlatList, TextInput, TouchableOpacity, ScrollView } from 'react-native';
import { styles } from './style';
import { Ionicons } from '@expo/vector-icons';

export default function RankingEstastistico({navigation}){

    return(
      <View style={styles.container}>
            <ScrollView 
                style={styles.scrollView}
                contentContainerStyle={styles.containerConteudo}
                showsVerticalScrollIndicator={false}
            >
                <View style={styles.nav}>
                    <TouchableOpacity 
                        style={styles.botaodevoltar}
                        onPress={() => navigation.navigate('HomeAdm', { screen: 'RankingAdm' })}
                    >
                        <Ionicons name="arrow-back" size={24} color="black" />
                    </TouchableOpacity>
                    <Text style={styles.titulo}>WORKFLOW</Text>
                    <View style={styles.espacoHeader} />
                </View>

            </ScrollView> 
        </View>
  );
}